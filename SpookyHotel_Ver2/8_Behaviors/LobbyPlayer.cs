// 로비에서의 플레이어 행동
class LobbyPlayer : Behavior
{
    public LobbyPlayer()
        : base()
    {

    }

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
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
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}