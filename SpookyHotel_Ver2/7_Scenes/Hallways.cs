class Hallways : RenderableScene
{
    GameObject player = null!;
    HallwaysPlayer playerHallwaysScript = null!;
    PlayerMovement playerMovementScript = null!;

    public override void Start()
    {
        base.Start();

        // 사운드
        if (!SoundManager.Instance.ResumeTrack("To Ponder"))
        {
            SoundManager.Instance.PlayTrack("to_ponder.mp3", "To Ponder");
        }

        // 리소스 로드
        ConsoleRenderer.Instance.LoadSprite("hallBase", new CharSpriteSize(10, 120), new CharSpriteCoords(), "halls.txt", false);
        ConsoleRenderer.Instance.LoadSprite("roomsLeftToClean", new CharSpriteSize(1, 14), new CharSpriteCoords(0, 7), "roomsLeftToCleanUI114.txt", false);

        // 층수에 맞게 리소스 변경
        CharSprite? nullable = ConsoleRenderer.Instance.GetSprite("hallBase");
        if (nullable != null)
        {
            // 문에 층별 호수 쓰기
            char currentFloor = (char)('0' + GameManager.Instance.playerCurrentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 10), currentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 26), currentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 42), currentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 77), currentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 93), currentFloor);
            nullable.SetCharByCoords(new CharSpriteCoords(2, 109), currentFloor);
        }

        /// 배경
        GameObject background = new GameObject("Background");
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>();
        background.GetComponent<CharSpriteRenderer>().CharSpriteKey = "hallBase";

        /// 플레이어 위치, 리밋 재설정
        player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;

        if (player != null)
        {
            player.Transform.position = new CharSpriteCoords(5, GameManager.Instance.hallwaysPosition);

            // 서있는 포즈로 바꿔주기
            player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";

            // 플에이어의 복도용 스크립트 붙이기
            playerMovementScript = player.AddComponent<PlayerMovement>();
            if (playerMovementScript != null)
            {
                playerMovementScript.MinRow = 2;
                playerMovementScript.MaxRow = 117;
            }
            playerHallwaysScript = player.AddComponent<HallwaysPlayer>();
        }

        /// 카메라에 스크립트 붙이기
        GameObject? camera = Find("Camera");
        if (camera != null)
        {
            camera.AddComponent<CameraMove>();
            CameraMove cameraMove = camera.GetComponent<CameraMove>();
            cameraMove.target = player;
            cameraMove.minRow = 0;
            cameraMove.maxRow = 90;
        }

        /// 엘리베이터 문열림
        {
            GameObject elevatorOpenDoor = new GameObject("OpenDoor", new CharSpriteCoords(3, 60));
            AddGameObject(elevatorOpenDoor);
            CharSpriteRenderer csr = elevatorOpenDoor.AddComponent<CharSpriteRenderer>();
            csr.CharSpriteKey = "openDoor";
            csr.enabled = Elevator.Instance.DoorOpen && GameManager.Instance.playerCurrentFloor == Elevator.Instance.CurrentFloor;
            elevatorOpenDoor.AddComponent<ElevatorDoorOpen_Hallways>().doorOpenSprite = csr;
        }

        /// 현재층 엘리베이터 예약 표시
        {
            GameObject selected = new GameObject("Selected", new CharSpriteCoords(2, 60));
            AddGameObject(selected);
            CharSpriteRenderer csr = selected.AddComponent<CharSpriteRenderer>();
            csr.CharSpriteKey = "buttonSelector";
            csr.enabled = Elevator.Instance.GetButton(GameManager.Instance.playerCurrentFloor);
            selected.AddComponent<HallwaysPressedButtonsDisplay>();
        }

        /// 엘리베이터 층수 표시
        {
            GameObject floorDisplay = new GameObject("FloorDisplay", new CharSpriteCoords(2, 60));
            AddGameObject(floorDisplay);
            CharRenderer cr = floorDisplay.AddComponent<CharRenderer>();
            cr.character = (char)('0' + Elevator.Instance.CurrentFloor);
            floorDisplay.AddComponent<HallwayFloorDisplay>().charRenderer = cr;
        }

        /// 엘리베이터 방향 표시
        {
            GameObject elevatorDirDisplay = new GameObject("DirDisplay", new CharSpriteCoords(4, 66));
            AddGameObject(elevatorDirDisplay);
            CharRenderer cr = elevatorDirDisplay.AddComponent<CharRenderer>();
            cr.character = '-';
            elevatorDirDisplay.AddComponent<HallwayDirDisplay>().charRenderer = cr;
        }

        /// 남은 방 UI
        if (GameManager.Instance.RoomsLeftToClean > 0)
        {
            GameObject ui = new GameObject("RoomsLeftToClean", new CharSpriteCoords(8, 23));
            AddGameObject(ui);
            CharSpriteRenderer cr = ui.AddComponent<CharSpriteRenderer>();
            cr.CharSpriteKey = "roomsLeftToClean";
            cr.enabled = GameManager.Instance.EnteredRoom;
            ui.ParentObject = Find("Camera");
            RoomsLeftToCleanUI uiScript = ui.AddComponent<RoomsLeftToCleanUI>();
            uiScript.renderer = cr;
            uiScript.UpdateNumber();
        }
    }

    public override void Exit()
    {
        SoundManager.Instance.PauseTrack("To Ponder");

        // 플레이어의 복도 전용 스크립트 제거
        if (player != null)
        {
            player.DestroyComponent(playerHallwaysScript);
            player.DestroyComponent(playerMovementScript);

            GameManager.Instance.hallwaysPosition = player.Transform.position.row;
        }

        base.Exit();
    }
}