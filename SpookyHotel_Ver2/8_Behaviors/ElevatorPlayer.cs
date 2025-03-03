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
    }
}