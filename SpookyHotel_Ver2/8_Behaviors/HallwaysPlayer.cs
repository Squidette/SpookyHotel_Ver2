class HallwaysPlayer : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        int enterRoom = -1;

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
        else if (gameObject.Transform.position.row >= 8 && gameObject.Transform.position.row <= 13) enterRoom = 1;
        else if (gameObject.Transform.position.row >= 24 && gameObject.Transform.position.row <= 29) enterRoom = 2;
        else if (gameObject.Transform.position.row >= 40 && gameObject.Transform.position.row <= 45) enterRoom = 3;
        else if (gameObject.Transform.position.row >= 75 && gameObject.Transform.position.row <= 80) enterRoom = 4;
        else if (gameObject.Transform.position.row >= 91 && gameObject.Transform.position.row <= 96) enterRoom = 5;
        else if (gameObject.Transform.position.row >= 107 && gameObject.Transform.position.row <= 112) enterRoom = 6;

        // 방 입장을 눌렀다면
        if (enterRoom != -1)
        {
            if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
            {
                GameManager.Instance.playerCurrentRoomNum = enterRoom;

                // 404호 입장 시도시
                if (GameManager.Instance.playerCurrentRoomNum == 4 && GameManager.Instance.playerCurrentFloor == 4)
                {
                    // 다른 방에 가지 않고 바로 여기로 왔다면 대사치고 돌려보내기
                    if (!GameManager.Instance.EnteredRoom)
                    {
                        SceneManager.Instance.LoadScene<HauntedRoomDenial>();
                    }
                    else // 다른방에서 튜토리얼이라도 보고왔다면 귀신들린 방으로 들여보내기
                    {
                        // 들어간적 없다면 입장
                        if (GameManager.Instance.lunaticDialogueStage == 0)
                        {
                            SceneManager.Instance.LoadScene<HauntedRoom>();
                        }
                        else // 이미 들어간적 없다면 대사치고 돌려보내기
                        {
                            SceneManager.Instance.LoadScene<HauntedRoomAfterMonologue>();
                        }
                    }
                }
                else // 일반 방 입장시
                {
                    GameManager.Instance.EnteredRoom = true;
                    SceneManager.Instance.LoadScene<Room>();
                }
            }
        }
    }
}