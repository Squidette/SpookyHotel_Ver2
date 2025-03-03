class ElevatorInside : RenderableScene
{
    GameObject player = null!;
    PlayerMovement playerMovementScript = null!;
    ElevatorPlayer playerElevatorScript = null!;

    public override void Start()
    {
        base.Start();

        //// 배경음악
        //if (!SoundManager.Instance.ResumeTrack("ElevMusic"))
        //{
        //    SoundManager.Instance.PlayTrack("In Limbo.mp3", "ElevMusic");
        //}

        // 리소스 로드
        ConsoleRenderer.Instance.LoadSprite("elevatorBase", new CharSpriteSize(10, 30), new CharSpriteCoords(), "elevatorBase.txt", false);
        ConsoleRenderer.Instance.LoadSprite("buttonSelector", new CharSpriteSize(1, 3), new CharSpriteCoords(0, 1), "buttonSelector.txt", true);

        /// 배경
        GameObject background = new GameObject("Background", new CharSpriteCoords());
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>().CharSpriteKey = "elevatorBase";

        /// 문열림
        GameObject openDoor = new GameObject("OpenDoor", new CharSpriteCoords(4, 14));
        AddGameObject(openDoor);
        CharSpriteRenderer csr = openDoor.AddComponent<CharSpriteRenderer>();
        csr.CharSpriteKey = "openDoor";
        csr.enabled = Elevator.Instance.DoorOpen;
        openDoor.AddComponent<ElevatorDoorOpen>().doorOpenSprite = csr;

        /// 눌린 버튼 표시
        GameObject[] selectedButtons = new GameObject[7];
        for (int i = 0; i < 7; i++)
        {
            selectedButtons[i] = new GameObject((i + 1).ToString() + "Selected", new CharSpriteCoords(2, 4 * i + 3));
            AddGameObject(selectedButtons[i]);
            selectedButtons[i].AddComponent<CharSpriteRenderer>().CharSpriteKey = "buttonSelector";
            selectedButtons[i].GetComponent<CharSpriteRenderer>().enabled = Elevator.Instance.GetButton(i + 1);

            if (i == 0) // 첫번째 오브젝트에만 대표로 스크립트를 붙여 주자
            {
                selectedButtons[0].AddComponent<ElevatorPressedButtonsDisplay>().pressedButtonDisplayers = selectedButtons;
            }
        }

        /// 현재 층 표시
        GameObject currentFloor = new GameObject("currentFloor", new CharSpriteCoords(1, 3));
        AddGameObject(currentFloor);
        currentFloor.AddComponent<CharRenderer>().character = '∇';
        currentFloor.AddComponent<ElevatorFloorDisplay>();

        /// 엘리베이터 방향 표시
        GameObject dirDisplay = new GameObject("DirDisplay", new CharSpriteCoords(5, 21));
        AddGameObject(dirDisplay);
        CharRenderer cr = dirDisplay.AddComponent<CharRenderer>();
        cr.character = '-';
        dirDisplay.AddComponent<HallwayDirDisplay>().charRenderer = cr; // 하는게 똑같으니까 복도걸 돌려쓰자

        /// 플레이어 위치, 리밋 재설정
        player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;

        if (player != null)
        {
            player.Transform.position = new CharSpriteCoords(6, 15);

            // 서있는 포즈로 바꿔주기
            player.GetComponent<CharSpriteRenderer>().CharSpriteKey = "player2";

            // 플에이어의 엘리베이터용 스크립트 붙이기
            playerMovementScript = player.AddComponent<PlayerMovement>();
            if (playerMovementScript != null)
            {
                playerMovementScript.MinRow = 2;
                playerMovementScript.MaxRow = 27;
            }
            playerElevatorScript = player.AddComponent<ElevatorPlayer>();
        }
        //player.GetComponent<Player>().MaxRow = 27;
    }

    //public override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //}

    public override void Exit()
    {
        //// 배경음악
        //SoundManager.Instance.PauseTrack("ElevMusic");

        // 플레이어의 로비 전용 스크립트 제거
        if (player != null)
        {
            player.DestroyComponent(playerMovementScript);
            player.DestroyComponent(playerElevatorScript);
        }

        base.Exit();
    }
}