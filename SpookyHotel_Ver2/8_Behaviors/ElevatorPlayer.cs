class ElevatorPlayer : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        InputManager im = InputManager.Instance;
        Elevator elevator = Elevator.Instance;

        // 엘리베이터 버튼 누르기
        if (im != null && elevator != null)
        {
            if (im.GetKey_Timed(ConsoleKey.D1))
            {
                elevator.PressButton(1);
            }
            else if (im.GetKey_Timed(ConsoleKey.D2))
            {
                elevator.PressButton(2);
            }
            else if (im.GetKey_Timed(ConsoleKey.D3))
            {
                elevator.PressButton(3);
            }
            else if (im.GetKey_Timed(ConsoleKey.D4))
            {
                elevator.PressButton(4);
            }
            else if (im.GetKey_Timed(ConsoleKey.D5))
            {
                elevator.PressButton(5);
            }
            else if (im.GetKey_Timed(ConsoleKey.D6))
            {
                elevator.PressButton(6);
            }
            else if (im.GetKey_Timed(ConsoleKey.D7))
            {
                elevator.PressButton(7);
            }
        }

        // 엘리베이터 문이 열려 있고, 플레이어가 문 앞에 서 있으며, 사용자가 스페이스를 눌렀을 때 로비나 복도로 나가기
        if (Elevator.Instance != null && Elevator.Instance.DoorOpen)
        {
            if (gameObject.Transform.position.row >= 10 && gameObject.Transform.position.row <= 18)
            {
                if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
                {
                    // 1층은 로비
                    if (Elevator.Instance.CurrentFloor == 1)
                    {
                        SceneManager.Instance.LoadScene<Lobby>();
                    }
                    // 2~6층은 일반홀
                    else if (2 <= Elevator.Instance.CurrentFloor && Elevator.Instance.CurrentFloor <= 6)
                    {
                        SceneManager.Instance.LoadScene<Hallways>();
                    }
                    // 7층은 귀신들린층
                    else if (Elevator.Instance.CurrentFloor == 7)
                    {
                        SceneManager.Instance.LoadScene<HauntedHallways>();
                    }
                }
            }
        }
        else if (Elevator.Instance != null && !Elevator.Instance.DoorOpen && Elevator.Instance.Direction == null) // 문이 닫혀 있다면 열기
        {
            if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
            {
                Elevator.Instance.DoorReserve = Elevator.DoorReserveState.WAITING_TO_OPEN;
            }
        }
    }
}