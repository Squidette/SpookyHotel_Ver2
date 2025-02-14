class Hallways : RenderableScene
{
    public override void Start()
    {
        base.Start();

        // 사운드
        if (!SoundManager.Instance.ResumeTrack("HallwaysMusic"))
        {
            SoundManager.Instance.PlayTrack("Cemetry Gates.mp3", "HallwaysMusic");
        }

        ConsoleRenderer.Instance.Draw("elevatorBase", new CharSpriteCoords(-1, -1));
    }

    public override void FixedUpdate()
    {
        if (InputManager.Instance.GetKey(ConsoleKey.C))
        {
            SceneManager.Instance.LoadScene<ElevatorInside>();
        }

        base.FixedUpdate();
    }

    public override void Exit()
    {
        SoundManager.Instance.PauseTrack("HallwaysMusic");

        base.Exit();
    }
}