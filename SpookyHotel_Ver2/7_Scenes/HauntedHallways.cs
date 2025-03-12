/// <summary>
/// 엔딩
/// </summary>
class HauntedHallways : RenderableScene
{
    GameObject player = null!;
    HauntedHallwaysPlayer playerHauntedHallwaysScript = null!;
    PlayerMovement playerMovementScript = null!;

    public override void Start()
    {
        base.Start();

        if (!SoundManager.Instance.ResumeTrack("DeathMusic"))
        {
            SoundManager.Instance.PlayTrack("intelligentsia.mp3", "DeathMusic");
        }

        ConsoleRenderer.Instance.LoadSprite("hallBase", new CharSpriteSize(10, 120), new CharSpriteCoords(), "halls.txt", false);
        ConsoleRenderer.Instance.LoadSprite("loop", new CharSpriteSize(10, 16), new CharSpriteCoords(), "looping7thdoor1016.txt", false);

        // 귀신들린 층은 호수 777로 통일하자
        CharSprite? nullable = ConsoleRenderer.Instance.GetSprite("hallBase");
        if (nullable != null)
        {
            int[] roomNumPositions = { 10, 26, 42, 77, 93, 109 };

            foreach (int roomNumPosition in roomNumPositions)
            {
                nullable.SetCharByCoords(new CharSpriteCoords(2, roomNumPosition), '7');
                nullable.SetCharByCoords(new CharSpriteCoords(2, roomNumPosition + 1), '7');
                nullable.SetCharByCoords(new CharSpriteCoords(2, roomNumPosition + 2), '7');
            }
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
            if (playerMovementScript != null) { playerMovementScript.LimitLess = true; }
            playerHauntedHallwaysScript = player.AddComponent<HauntedHallwaysPlayer>();
        }

        /// 카메라에 스크립트 붙이기
        GameObject? camera = Find("Camera");
        if (camera != null)
        {
            camera.AddComponent<CameraMove>();
            CameraMove cameraMove = camera.GetComponent<CameraMove>();
            cameraMove.target = player;
            cameraMove.limitLess = true;
        }

        /// 엘리베이터 층수 표시
        {
            GameObject floorDisplay = new GameObject("FloorDisplay", new CharSpriteCoords(2, 60));
            AddGameObject(floorDisplay);
            floorDisplay.AddComponent<CharRenderer>().character = 'X';
        }

        /// 엘리베이터 방향 표시
        {
            GameObject elevatorDirDisplay = new GameObject("DirDisplay", new CharSpriteCoords(4, 86)); //66
            AddGameObject(elevatorDirDisplay);
            elevatorDirDisplay.AddComponent<CharRenderer>().character = 'X';
        }

        /// 끝없는 복도표현
        GameObject loopingHallway1 = new GameObject("LoopingHallway1", new CharSpriteCoords(0, 86));
        AddGameObject(loopingHallway1);
        loopingHallway1.AddComponent<CharSpriteRenderer>().CharSpriteKey = "loop";

        GameObject loopingHallway2 = new GameObject("LoopingHallway2", new CharSpriteCoords(0, 102));
        AddGameObject(loopingHallway2);
        loopingHallway2.AddComponent<CharSpriteRenderer>().CharSpriteKey = "loop";

        GameObject loopingHallway3 = new GameObject("LoopingHallway3", new CharSpriteCoords(0, 118));
        AddGameObject(loopingHallway3);
        loopingHallway3.AddComponent<CharSpriteRenderer>().CharSpriteKey = "loop";

        LoopingHallways lh = loopingHallway2.AddComponent<LoopingHallways>();
        lh.player = player!.Transform;
        lh.SetThreeLoopingBackgroundObjects(loopingHallway1, loopingHallway2, loopingHallway3);

        //lh.loop1 = loopingHallway1.Transform;
        //lh.loop2 = loopingHallway2.Transform;
    }

    public override void Exit()
    {
        // 플레이어의 복도 전용 스크립트 제거
        if (player != null)
        {
            player.DestroyComponent(playerHauntedHallwaysScript);
            player.DestroyComponent(playerMovementScript);

            GameManager.Instance.hallwaysPosition = player.Transform.position.row;
        }

        base.Exit();
    }
}