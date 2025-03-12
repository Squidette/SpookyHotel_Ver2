/// <summary>
/// 첫 번째 씬으로, 1층 로비씬
/// </summary>
class Lobby : RenderableScene
{
    GameObject player = null!;
    LobbyPlayer playerLobbyScript = null!;
    PlayerMovement playerMovementScript = null!;

    public override void Start()
    {
        base.Start();

        // 배경음악
        if (!SoundManager.Instance.ResumeTrack("To Ponder"))
        {
            SoundManager.Instance.PlayTrack("to_ponder.mp3", "To Ponder");
        }

        // 리소스 로드
        ConsoleRenderer.Instance.LoadSprite("lobbyBase", new CharSpriteSize(10, 85), new CharSpriteCoords(), "lobby1085.txt", false);
        ConsoleRenderer.Instance.LoadSprite("player1", new CharSpriteSize(3, 3), new CharSpriteCoords(1, 1), "player1.txt", true);
        ConsoleRenderer.Instance.LoadSprite("player2", new CharSpriteSize(3, 3), new CharSpriteCoords(1, 1), "player2.txt", true);
        ConsoleRenderer.Instance.LoadSprite("openDoor", new CharSpriteSize(4, 1), new CharSpriteCoords(), "opendoor41.txt", false);

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

        /// 엘리베이터
        if (!Elevator.Created)
        {
            GameObject elevator = new GameObject("Elevator");
            AddGameObject(elevator);
            elevator.AddComponent<Elevator>();
        }

        /// 엘리베이터 문열림
        {
            GameObject elevatorOpenDoor = new GameObject("OpenDoor", new CharSpriteCoords(3, 70));
            AddGameObject(elevatorOpenDoor);
            CharSpriteRenderer csr = elevatorOpenDoor.AddComponent<CharSpriteRenderer>();
            csr.CharSpriteKey = "openDoor";
            csr.enabled = Elevator.Instance.DoorOpen && Elevator.Instance.CurrentFloor == 1;
            elevatorOpenDoor.AddComponent<ElevatorDoorOpen_Hallways>().doorOpenSprite = csr;
        }

        /// 현재층 엘리베이터 예약 표시
        {
            GameObject selected = new GameObject("Selected", new CharSpriteCoords(2, 70));
            AddGameObject(selected);
            CharSpriteRenderer csr = selected.AddComponent<CharSpriteRenderer>();
            csr.CharSpriteKey = "buttonSelector";
            csr.enabled = Elevator.Instance.GetButton(GameManager.Instance.playerCurrentFloor);
            selected.AddComponent<HallwaysPressedButtonsDisplay>();
        }

        /// 엘리베이터 층수 표시
        {
            GameObject floorDisplay = new GameObject("FloorDisplay", new CharSpriteCoords(2, 70));
            AddGameObject(floorDisplay);
            CharRenderer cr = floorDisplay.AddComponent<CharRenderer>();
            cr.character = (char)('0' + Elevator.Instance.CurrentFloor);
            floorDisplay.AddComponent<HallwayFloorDisplay>().charRenderer = cr;
        }

        /// 엘리베이터 방향 표시
        {
            GameObject elevatorDirDisplay = new GameObject("DirDisplay", new CharSpriteCoords(4, 76));
            AddGameObject(elevatorDirDisplay);
            CharRenderer cr = elevatorDirDisplay.AddComponent<CharRenderer>();
            cr.character = '-';
            elevatorDirDisplay.AddComponent<HallwayDirDisplay>().charRenderer = cr;
        }

        /// 플레이어
        // 씬이 처음이면 생성
        if (!Player.Created)
        {
            player = new GameObject("Player", new CharSpriteCoords(5, 16)); // 원래 16
            AddGameObject(player);
            player.AddComponent<CharSpriteRenderer>();
            player.AddComponent<Player>();  // 여기에 DontDestroyOnLoadScene으로 보내는 스크립트가 있다
        }
        else // 씬이 처음이 아닌 경우
        {
            // 아니면 DontDestroyOnLoadScene에 들어가있는 플레이어 찾기
            player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;
            player.Transform.position = new CharSpriteCoords(5, GameManager.Instance.lobbyPosition); // 마지막으로 나갔던 위치에 두기
        }

        // 서있는 포즈로 바꿔주기
        player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";

        // 플에이어의 로비용 스크립트 붙이기
        playerMovementScript = player.AddComponent<PlayerMovement>();
        if (playerMovementScript != null)
        {
            playerMovementScript.MinRow = 2;
            playerMovementScript.MaxRow = 82;
        }

        playerLobbyScript = player.AddComponent<LobbyPlayer>();

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

        //// debug
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.D2))
        //{
        //    GameManager.Instance.playerCurrentFloor = 2;
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.D3))
        //{
        //    GameManager.Instance.playerCurrentFloor = 3;
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.D4))
        //{
        //    GameManager.Instance.playerCurrentFloor = 4;
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.D5))
        //{
        //    GameManager.Instance.playerCurrentFloor = 5;
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
        //if (InputManager.Instance.GetKey_Timed(ConsoleKey.D6))
        //{
        //    GameManager.Instance.playerCurrentFloor = 6;
        //    SceneManager.Instance.LoadScene<Hallways>();
        //}
    }

    public override void Exit()
    {
        // 배경음악
        SoundManager.Instance.PauseTrack("To Ponder");

        // 플레이어의 로비 전용 스크립트 제거
        if (player != null)
        {
            player.DestroyComponent(playerLobbyScript);
            player.DestroyComponent(playerMovementScript);

            // 마지막 x좌표 저장
            GameManager.Instance.lobbyPosition = player.Transform.position.row;
        }

        base.Exit();
    }
}