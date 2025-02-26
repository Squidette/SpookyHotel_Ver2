/// <summary>
/// 첫 번째 씬
/// </summary>
class Lobby : RenderableScene
{
    GameObject player = null!;
    LobbyPlayer playerLobbyScript = null!;

    public override void Start()
    {
        base.Start();

        // 배경음악
        if (!SoundManager.Instance.ResumeTrack("LobbyMusic"))
        {
            SoundManager.Instance.PlayTrack("Ill Wind.mp3", "LobbyMusic");
        }

        ConsoleRenderer.Instance.LoadSprite("lobbyBase", new CharSpriteSize(10, 85), new CharSpriteCoords(), "lobby1085.txt", false);
        ConsoleRenderer.Instance.LoadSprite("player1", new CharSpriteSize(3, 3), new CharSpriteCoords(1, 1), "player1.txt", true);
        ConsoleRenderer.Instance.LoadSprite("player2", new CharSpriteSize(3, 3), new CharSpriteCoords(1, 1), "player2.txt", true);

        /// 게임매니저
        // 씬이 처음이면 생성
        if (!GameManager.Created)
        {
            GameObject gameManager = new GameObject("GameManager");
            AddGameObject(gameManager);
            gameManager.AddComponent<GameManager>();
        }

        /// 배경
        GameObject background = new GameObject("Background");
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>();
        background.GetComponent<CharSpriteRenderer>().CharSpriteKey = "lobbyBase";

        /// 플레이어
        // 씬이 처음이면 생성
        if (!PlayerMovement.Created)
        {
            player = new GameObject("Player", new CharSpriteCoords(5, 16));
            AddGameObject(player);
            player.AddComponent<CharSpriteRenderer>();
            player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";
            player.AddComponent<PlayerMovement>();  // 여기에 DontDestroyOnLoadScene으로 보내는 스크립트가 있다
        }
        else // 플레이어가 위층에서 내려오는 경우
        {
            // 아니면 DontDestroyOnLoadScene에 들어가있는 플레이어 찾기
            player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;
            //player.Transform.position = new CharSpriteCoords(5, 69); // 엘리베이터 앞에 위치
        }

        player.GetComponent<PlayerMovement>().MinRow = 2;
        player.GetComponent<PlayerMovement>().MaxRow = 82;

        // 이 씬에서만 플레이어한테 붙일 스크립트
        playerLobbyScript = player.AddComponent<LobbyPlayer>();
        //if (playerLobbyScript != null)
        //{
        //    playerLobbyScript.HotelFrontMan = hotelFrontMan.GetComponent<HotelFrontMan>();
        //}

        /// 카메라에 스크립트 붙이기
        GameObject? camera = Find("Camera");
        if (camera != null)
        {
            camera.AddComponent<CameraMove>();
            CameraMove cameraMove = camera.GetComponent<CameraMove>();
            cameraMove.target = player;
            cameraMove.minRow = 0;
            cameraMove.maxRow = 55;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        // 배경음악
        SoundManager.Instance.PauseTrack("LobbyMusic");

        // 플레이어의 로비 전용 스크립트 제거
        if (player != null)
        {
            player.DestroyComponent(playerLobbyScript);
        }

        base.Exit();
    }
}