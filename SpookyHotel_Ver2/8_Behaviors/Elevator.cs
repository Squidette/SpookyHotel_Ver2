class Elevator : Behavior
{
    /// 유사 싱글톤
    static bool created = false;
    public static bool Created
    {
        get { return created; }
    }

    static Elevator instance = null!;
    public static Elevator Instance
    {
        get { return instance; }
    }

    ///  엘리베이터 내용

    // 현재 층
    int currentFloor;
    public int CurrentFloor
    {
        get => currentFloor;
        set => currentFloor = Math.Clamp(value, 1, 7);
    }

    // 진행 방향 - 현실의 엘리베이터에 있는 그 올라갑니다, 내려갑니다 진행방향   // null: 멈춤, true: 위, false: 아래
    bool? direction;
    public bool? Direction
    {
        get => direction;
        set
        {
            if (value != null) moveTimer.Reset();

            // debug
            if (value == null) { Debug.Log("direction set null"); }
            else if (value == true) { Debug.Log("direction set true"); }
            else if (value == false) { Debug.Log("direction set false"); }

            direction = value;
        }
    }

    // 현재 층에 멈춰 있는지?
    bool stopped;
    public bool Stopped
    {
        get => stopped;
        set
        {
            if (value != stopped)
            {
                stopped = value;
                if (!stopped) moveTimer?.Reset();

                // debug
                Debug.Log(stopped ? "Stopped" : "Moving");
            }
        }
    }

    // 문이 열려 있는지?
    bool doorOpen;
    public bool DoorOpen
    {
        get => doorOpen;
        set
        {
            if (doorOpen != value)
            {
                doorOpen = value;

                RaiseDoorEvent?.Invoke(this, new ElevatorDoorOpenEventArgs(doorOpen));
                waitTimer.Reset();

                // debug
                Debug.Log(doorOpen ? "DOOR OPEN" : "DOOR CLOSED");
            }
        }
    }

    // 문열기 예약 상태 (doorTimer에 설정된 시간만큼 문이 여닫히는 시간이 걸린다는 설정)
    public enum DoorReserveState { NONE, WAITING_TO_OPEN, WAITING_TO_CLOSE }

    DoorReserveState doorReserveState;
    public DoorReserveState DoorReserve
    {
        set
        {
            if (doorReserveState != value)
            {
                if (value != DoorReserveState.NONE) doorTimer.Reset();
                doorReserveState = value;

                // debug
                switch (value)
                {
                    case DoorReserveState.NONE:
                        Debug.Log("DoorReserveState Set To None");
                        break;
                    case DoorReserveState.WAITING_TO_OPEN:
                        Debug.Log("DoorReserveState Set To Waiting To Open");
                        break;
                    case DoorReserveState.WAITING_TO_CLOSE:
                        Debug.Log("DoorReserveState Set To Waiting To Close");
                        break;
                }
            }
        }
    }

    // stopped 이벤트 발송
    public class ElevatorDoorOpenEventArgs : EventArgs
    {
        public bool open;
        public ElevatorDoorOpenEventArgs(bool open) => this.open = open;
    }
    public event EventHandler<ElevatorDoorOpenEventArgs>? RaiseDoorEvent;

    // 다음 층으로 가기까지 걸릴 시간
    const int moveSpeed = 70;
    FCTimer moveTimer;

    // 멈춰 있지 않을 때 한 층에서 머무를 시간
    const int waitSpeed = 170;
    FCTimer waitTimer;

    // 문이 여닫히는데 걸리는 시간
    const int doorSpeed = 30;
    FCTimer doorTimer;

    // 눌린 버튼: true면 눌림, false면 꺼짐
    bool[] pressedButtons;
    public bool GetButton(int floor)
    {
        if (floor < 1 || floor > 7) return false;
        else return pressedButtons[floor - 1];
    }
    bool SetButton(int floor, bool press = true /*false - unpress*/)
    {
        if (pressedButtons != null && pressedButtons.Length >= floor)
        {
            if (pressedButtons[floor - 1] != press)
            {
                pressedButtons[floor - 1] = press;
                RaiseButtonPressEvent?.Invoke(this, new PressedButtonEventArgs(floor, press));

                return true;
            }
            else return false;
        }
        else return false;
    }
    bool HasPressedButtonOver(int floor)
    {
        if (floor < 1 || floor >= 7) return false;

        bool hasButtonOver = false;

        for (int i = floor; i < pressedButtons.Length; i++)
        {
            if (pressedButtons[i])
            {
                hasButtonOver = true;
                Debug.Log("something pressed over " + floor);
                break;
            }
        }
        return hasButtonOver;
    }

    bool HasPressedButtonUnder(int floor)
    {
        if (floor <= 1 || floor > 7) return false;

        bool hasButtonUnder = false;

        for (int i = 0; i < floor - 1; i++)
        {
            if (pressedButtons[i])
            {
                hasButtonUnder = true;
                Debug.Log("something pressed under " + floor);
                break;
            }
        }
        return hasButtonUnder;
    }

    // 버튼이 눌릴 때 이벤트 발송
    public class PressedButtonEventArgs : EventArgs
    {
        public int floor;
        public bool pressed;
        public PressedButtonEventArgs(int floor, bool pressed)
        {
            this.floor = floor;
            this.pressed = pressed;
        }
    }
    public event EventHandler<PressedButtonEventArgs>? RaiseButtonPressEvent;

    public Elevator()
    {
        // 유사 싱글톤
        created = true;
        instance = this;

        // 엘리베이터 관련
        currentFloor = 1;
        pressedButtons = new bool[7] { false, false, false, false, false, false, false };
        direction = null;
        stopped = true;
        doorOpen = false;
        doorReserveState = DoorReserveState.NONE;

        moveTimer = new FCTimer(moveSpeed);
        waitTimer = new FCTimer(waitSpeed);
        doorTimer = new FCTimer(doorSpeed);
    }

    // 층 버튼 누르기. 다른 스크립트에서 접근하여 사용
    public bool PressButton(int inputFloor)
    {
        if (inputFloor < 1 || inputFloor > 7) return false;
        if (currentFloor == inputFloor && direction == null) return false;

        if (!SetButton(inputFloor)) return false;

        if (direction == null)
        {
            Direction = currentFloor < inputFloor ? true : false;
            DoorReserve = DoorReserveState.WAITING_TO_CLOSE;
        }
        return true;
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ProcessMovement();
        ProcessDoor();
    }

    void ProcessMovement()
    {
        // 엘리베이터의 방향이 있다면
        if (direction != null && !doorOpen && doorReserveState == DoorReserveState.NONE)
        {
            // (움직이고 있지 않다면) 엘리베이터 움직이기
            Stopped = false;

            // (움직이고 있다면) 엘리베이터 이동 & 멈추기
            // 시간이 지났다면
            if (!stopped && moveTimer.Past)
            {
                // 층이 하나 올라가거나 내려간다
                CurrentFloor = direction.Value ? currentFloor + 1 : currentFloor - 1;

                // 버튼이 눌린 곳이라면 멈추고 문 열기
                if (pressedButtons[currentFloor - 1])
                {
                    SetButton(currentFloor, false);
                    Stopped = true;
                    DoorReserve = DoorReserveState.WAITING_TO_OPEN;
                }
                // 눌리지 않은 곳이라면 이동 타이머만 리셋하고 계속 가기
                else moveTimer.Reset();

                /// 엘리베이터의 다음 방향을 결정
                // 올라가는 중이었고, 더이상 눌린 높은 층이 없는 경우 direction up을 벗어남
                if (direction == true && !HasPressedButtonOver(currentFloor))
                {
                    // 현재보다 낮은 층이 눌렸는지 여부에 따라 direction이 false 혹은 null
                    Direction = HasPressedButtonUnder(currentFloor) ? false : null;
                }
                // 내려가는 중이었고, 더이상 눌린 낮은 층이 없는 경우 direction down을 벗어남
                else if (direction == false && !HasPressedButtonUnder(currentFloor))
                {
                    // 현재보다 높은 층이 눌렸는지 여부에 따라 direction이 true 혹은 null
                    Direction = HasPressedButtonOver(currentFloor) ? true : null;
                }
            }
        }
    }

    void ProcessDoor()
    {
        // 문 열림/닫힘이 예약되었고, 문 타이머의 시간이 지났다면 doorOpen 값을 바꿔 주고 NONE상태로 돌아가기
        switch (doorReserveState)
        {
            case DoorReserveState.WAITING_TO_OPEN:
                if (doorTimer.Past)
                {
                    DoorOpen = true;
                    DoorReserve = DoorReserveState.NONE;
                }
                break;
            case DoorReserveState.WAITING_TO_CLOSE:
                if (doorTimer.Past)
                {
                    DoorOpen = false;
                    DoorReserve = DoorReserveState.NONE;
                }
                break;
        }

        // 문이 대기 타이머 시간만큼 열려있었다면 닫힘 예약 상태로 바꾸기
        if (doorOpen && waitTimer.Past) DoorReserve = DoorReserveState.WAITING_TO_CLOSE;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        FCTimer.ReleaseTimer(moveTimer);
        FCTimer.ReleaseTimer(waitTimer);
        FCTimer.ReleaseTimer(doorTimer);
    }
}