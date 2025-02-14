using System.Numerics;

class ElevatorInside : RenderableScene
{
    public override void Start()
    {
        base.Start();

        ConsoleRenderer.Instance.LoadSprite("elevatorBase", new CharSpriteSize(10, 30), new CharSpriteCoords(), "elevatorBase.txt", false);

        GameObject background = new GameObject("background", new CharSpriteCoords());
        AddGameObject(background);
        background.AddComponent<CharSpriteRenderer>();
        background.GetComponent<CharSpriteRenderer>().CharSpriteKey = "elevatorBase";

        // 플레이어 위치, 리밋 재설정
        GameObject player = SceneManager.Instance.DontDestroyOnLoadScene.Find("Player")!;
        player.Transform.position = new CharSpriteCoords(6, 15);
        player.GetComponent<Player>().MaxRow = 27;

        // 사운드
        if (!SoundManager.Instance.ResumeTrack("LobbyMusic"))
        {
            SoundManager.Instance.PlayTrack("In Limbo.mp3", "LobbyMusic");
        }
    }

    public override void FixedUpdate()
    {
        if (InputManager.Instance.GetKey(ConsoleKey.Spacebar))
        {
            SceneManager.Instance.LoadScene<Lobby>();
        }

        base.FixedUpdate();
    }

    public override void Exit()
    {
        // 사운드
        SoundManager.Instance.PauseTrack("LobbyMusic");

        ConsoleRenderer.Instance.Draw("elevatorBase", new CharSpriteCoords());

        base.Exit();
    }
}