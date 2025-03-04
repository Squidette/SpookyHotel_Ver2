class HallwaysPlayer : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 엘리베이터 입장하기
        if (gameObject.Transform.position.row >= 56 && gameObject.Transform.position.row <= 64)
        {
            if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
            {
                // 엘리베이터가 멈춰 있고, 현재 층이면 엘리베이터 입장
                if (Elevator.Instance.DoorOpen && Elevator.Instance.CurrentFloor == GameManager.Instance.playerCurrentFloor)
                {
                    SceneManager.Instance.LoadScene<ElevatorInside>();
                }
                // 아니면 현재 층으로 엘리베이터 호출
                else
                {
                    Elevator.Instance.PressButton(GameManager.Instance.playerCurrentFloor);
                    if (Elevator.Instance.Stopped)
                    {
                        Elevator.Instance.DoorReserve = Elevator.DoorReserveState.WAITING_TO_OPEN;
                    }
                }
            }
        }
    }
}