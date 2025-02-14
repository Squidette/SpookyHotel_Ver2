class Dialogue : RenderableScene
{
    public override void Start()
    {
        SpookyHotel_Ver2.renderMode = SpookyHotel_Ver2.RenderMode.KRTEXTPRINTER;
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        SpookyHotel_Ver2.renderMode = SpookyHotel_Ver2.RenderMode.CONSOLERENDERER;
        base.Exit();
    }
}