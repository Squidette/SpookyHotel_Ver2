// 로비에서의 플레이어 행동
class LobbyPlayer : Behavior
{
    //public override void Start()
    //{
    //    base.Start();
    //}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 호텔 프론트맨에게 말하기
        if (gameObject.Transform.position.row >= 37 && gameObject.Transform.position.row <= 49)
        {
            // 처음 말을 건다면, 게임 스토리 설명
            if (!GameManager.Instance.talkedToFrontMan)
            {
                if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
                {
                    SceneManager.Instance.LoadScene<HotelFrontManDialogue>();
                    GameManager.Instance.talkedToFrontMan = true;
                }
            }
            // 두 번째 이상 말을 건다면, 코멘트 정도만
            else
            {
                if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
                {
                    SceneManager.Instance.LoadScene<HotelFrontManComments>();
                }
            }
        }

        // 엘리베이터 입장하기
        if (gameObject.Transform.position.row >= 66 && gameObject.Transform.position.row <= 74)
        {
            if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
            {
                // 엘리베이터가 멈춰 있고, 현재 층이면 엘리베이터 입장
                if (Elevator.Instance.DoorOpen && Elevator.Instance.CurrentFloor == 1)
                {
                    SceneManager.Instance.LoadScene<ElevatorInside>();
                }
                // 아니면 현재 층으로 엘리베이터 호출
                else
                {
                    Elevator.Instance.PressButton(1);
                    Elevator.Instance.DoorReserve = Elevator.DoorReserveState.WAITING_TO_OPEN;
                }
            }
        }
    }

    //public override void OnDestroy()
    //{
    //    base.OnDestroy();
    //}
}