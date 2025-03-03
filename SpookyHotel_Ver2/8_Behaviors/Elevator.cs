using System.Drawing;

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
        get
        {
            return currentFloor;
        }
        set
        {
            if (value > 7) currentFloor = 7;
            else if (value < 1) currentFloor = 1;
            else currentFloor = value;
        }
    }

    // 진행 방향 - 현실의 엘리베이터에 있는 그 화살표   // null: 멈춤, true: 위, false: 아래
    bool? direction;
    public bool? Direction
    {
        get
        {
            return direction;
        }
        set
        {
            if (value == null)
            {
                Debug.Log("direction set null");
            }
            else if (value == true)
            {
                Debug.Log("direction set true");
            }
            else if (value == false)
            {
                Debug.Log("direction set false");
            }
            moveTimer.Reset();
            direction = value;
        }
    }

    // 현재 층에 멈춰 있는지?
    bool stopped;
    public bool Stopped
    {
        get
        {
            return stopped;
        }
        set
        {
            if (value != stopped)
            {
                stopped = value;
                RaiseStoppedEvent?.Invoke(this, new ElevatorStoppedEventArgs(stopped));

                if (stopped) Debug.Log("stopped");
                else Debug.Log("moving");

                if (stopped) waitTimer?.Reset();
                else moveTimer?.Reset();
            }
        }
    }

    // stopped 이벤트 발송
    public class ElevatorStoppedEventArgs : EventArgs
    {
        public bool stopped;
        public ElevatorStoppedEventArgs(bool stopped)
        {
            this.stopped = stopped;
        }
    }
    public event EventHandler<ElevatorStoppedEventArgs>? RaiseStoppedEvent;

    // 다음 층으로 가기까지 걸릴 시간
    const int moveSpeed = 70;
    FCTimer moveTimer;

    // 멈춰 있지 않을 때 한 층에서 머무를 시간
    const int waitSpeed = 370;
    FCTimer waitTimer;

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

        pressedButtons = new bool[7] { false, false, false, false, false, false, false };

        direction = null;
        stopped = true;

        currentFloor = 1;

        moveTimer = new FCTimer(moveSpeed);
        waitTimer = new FCTimer(waitSpeed);
    }

    public bool PressButton(int inputFloor)
    {
        if (inputFloor < 1 || inputFloor > 7) return false;
        if (currentFloor == inputFloor) return false;

        bool pressed = SetButton(inputFloor);

        // 엘리베이터가 멈춰있다면 움직이기
        if (pressed)
        {
            if (direction == null)
            {
                if (currentFloor < inputFloor)
                {
                    Direction = true; // 위로
                }
                else if (currentFloor > inputFloor)
                {
                    Direction = false; // 아래로
                }
            }
            return true;
        }
        else return false;
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (direction != null)
        {
            if (stopped && waitTimer.Past)
            {
                Stopped = false;
                Debug.Log("멈춰 있던 엘리베이터가 다시 움직입니다");
            }
            else if (!stopped && moveTimer.Past)
            {
                CurrentFloor = direction.Value ? currentFloor + 1 : currentFloor - 1;

                // 버튼이 눌린 곳이라면 멈추기
                if (pressedButtons[currentFloor - 1])
                {
                    SetButton(currentFloor, false);
                    Stopped = true;
                }
                // 눌리지 않은 곳이라면 타이머만 리셋하고 그냥 가기
                else
                {
                    moveTimer.Reset();
                }

                // 다음 Direction 결정
                if (direction == true && !HasPressedButtonOver(currentFloor))
                {
                    if (HasPressedButtonUnder(currentFloor))
                    {
                        Direction = false;
                    }
                    else
                    {
                        Direction = null;
                    }
                }
                else if (direction == false && !HasPressedButtonUnder(currentFloor))
                {
                    if (HasPressedButtonOver(currentFloor))
                    {
                        Direction = true;
                    }
                    else
                    {
                        Direction = null;
                    }
                }
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        FCTimer.ReleaseTimer(moveTimer);
        FCTimer.ReleaseTimer(waitTimer);
    }
}