class Room : RenderableScene
{
    GameObject player = null!;

    public override void Start()
    {
        base.Start();

        ConsoleRenderer.Instance.LoadSprite("roomBase", new CharSpriteSize(10, 30), new CharSpriteCoords(), "roomBase.txt", false);
        ConsoleRenderer.Instance.LoadSprite("cleanBed", new CharSpriteSize(2, 10), new CharSpriteCoords(), "cleanSheet210.txt", false);
        ConsoleRenderer.Instance.LoadSprite("dirtyBed", new CharSpriteSize(2, 10), new CharSpriteCoords(), "dirtySheet210.txt", false);
        ConsoleRenderer.Instance.LoadSprite("cleanPlant", new CharSpriteSize(3, 4), new CharSpriteCoords(2, 0), "cleanPlant34.txt", false);
        ConsoleRenderer.Instance.LoadSprite("dirtyPlant", new CharSpriteSize(2, 5), new CharSpriteCoords(), "dirtyPlant25.txt", false);
        ConsoleRenderer.Instance.LoadSprite("cleanWindow", new CharSpriteSize(3, 9), new CharSpriteCoords(), "sqeakyWindow39.txt", false);
        ConsoleRenderer.Instance.LoadSprite("dirtyWindow", new CharSpriteSize(3, 9), new CharSpriteCoords(), "dirtyWindow39.txt", false);

        /// 배경
        GameObject background = new GameObject("Background");
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>().CharSpriteKey = "roomBase";

        /// 사물 그리기
        if (GameManager.Instance != null)
        {
            GameManager.RoomInfo currentRoomInfo = GameManager.Instance.GetCurrentRoomInfo();

            GameObject bed = new GameObject("Bed", new CharSpriteCoords(5, 4));
            AddGameObject(bed);
            bed.AddComponent<CharSpriteRenderer>().CharSpriteKey = currentRoomInfo.BedClean ? "cleanBed" : "dirtyBed";

            GameObject plant = new GameObject("Plant", new CharSpriteCoords(6, 24));
            AddGameObject(plant);
            plant.AddComponent<CharSpriteRenderer>().CharSpriteKey = currentRoomInfo.PlantClean ? "cleanPlant" : "dirtyPlant";

            GameObject window = new GameObject("Window", new CharSpriteCoords(1, 5));
            AddGameObject(window);
            window.AddComponent<CharSpriteRenderer>().CharSpriteKey = currentRoomInfo.WindowClean ? "cleanWindow" : "dirtyWindow";

            RoomCleaning rc = window.AddComponent<RoomCleaning>(); // 대표로 창문에 붙여두자
            if (rc != null)
            {
                rc.bedRenderer = bed.GetComponent<CharSpriteRenderer>();
                rc.plantRenderer = plant.GetComponent<CharSpriteRenderer>();
                rc.windowRenderer = window.GetComponent<CharSpriteRenderer>();
            }
        }

        /// 플레이어 설정
        player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;

        if (player != null)
        {
            player.Transform.position = new CharSpriteCoords(5, 19);

            // 서있는 포즈로 바꿔주기
            player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // 스페이스 누르면 나가기
        if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
        {
            // 치워야하는 방 수 업데이트
            if (GameManager.Instance.EnteredRoom)
            {
                GameManager.Instance.CountUpCleanRooms();
            }

            // 처음으로 다 치웠다면 대사 재생
            if (GameManager.Instance.RoomsLeftToClean == 0 && !CleanDone.shown)
            {
                SceneManager.Instance.LoadScene<CleanDone>();
            }
            else // 아니면 그냥 나가기
            {
                SceneManager.Instance.LoadScene<Hallways>();
            }
        }
    }

    public override void Exit()
    {


        base.Exit();
    }
}