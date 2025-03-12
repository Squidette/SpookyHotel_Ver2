/// <summary>
/// 귀신들린 방
/// </summary>
class HauntedRoom : RenderableScene
{
    GameObject player = null!;

    public override void Start()
    {
        base.Start();

        /// 배경
        GameObject background = new GameObject("Background");
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>().CharSpriteKey = "roomBase";

        /// 사물 그리기
        if (GameManager.Instance != null)
        {
            GameObject bed = new GameObject("Bed", new CharSpriteCoords(5, 4));
            AddGameObject(bed);
            bed.AddComponent<CharSpriteRenderer>().CharSpriteKey = "dirtyBed";

            GameObject plant = new GameObject("Plant", new CharSpriteCoords(6, 24));
            AddGameObject(plant);
            plant.AddComponent<CharSpriteRenderer>().CharSpriteKey = "dirtyPlant";

            GameObject window = new GameObject("Window", new CharSpriteCoords(1, 5));
            AddGameObject(window);
            window.AddComponent<CharSpriteRenderer>().CharSpriteKey = "dirtyWindow";
        }

        /// 플레이어 위치, 리밋 재설정
        player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;

        if (player != null)
        {
            player.Transform.position = new CharSpriteCoords(5, 19);

            // 서있는 포즈로 바꿔주기
            player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";
        }

        /// 미친놈
        GameObject lunatic = new GameObject("Lunatic", new CharSpriteCoords(4, 8));
        AddGameObject(lunatic);
        lunatic.AddComponent<CharSpriteRenderer>().CharSpriteKey = "player2";
        lunatic.AddComponent<LunaticSpeak>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //// 스페이스 누르면 나가기
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
        //{
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
    }

    public override void Exit()
    {
        // 나가는 순간 7층이 해금?되며 어디선가 엘리베이터 내부의 7증 버튼이 자동으로 눌린다
        if (Elevator.Instance != null)
        {
            Elevator.Instance.SeventhFloorHidden = false;
            Elevator.Instance.PressButton(7);
        }

        base.Exit();
    }
}