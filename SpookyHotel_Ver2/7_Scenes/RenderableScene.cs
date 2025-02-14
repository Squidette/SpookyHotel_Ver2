/// <summary>
/// 렌더 가능한 일반 씬
/// </summary>
class RenderableScene : Scene
{
    protected GameObject camera;
    public GameObject Camera
    {
        get { return camera; }
    }

    public RenderableScene()
        : base()
    {
        camera = new GameObject("Camera", new CharSpriteCoords());
        gameObjects.Add(camera);
    }

    /// <summary>이 씬이 로드될때마다 한 번 실행</summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>매 FixedUpdate에서 지속적으로 실행</summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>이 씬을 나갈때마다 한 번 실행</summary>
    public override void Exit()
    {
        base.Exit();
    }
}