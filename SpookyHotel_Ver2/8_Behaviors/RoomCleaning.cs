/// <summary>
/// 방 내부 청소
/// </summary>
class RoomCleaning : Behavior
{
    public CharSpriteRenderer bedRenderer = null!;
    public CharSpriteRenderer plantRenderer = null!;
    public CharSpriteRenderer windowRenderer = null!;

    public override void Start()
    {
        base.Start();

        GameManager.Instance.BedCleanEvent += OnBedCleaned;
        GameManager.Instance.PlantCleanEvent += OnPlantCleaned;
        GameManager.Instance.WindowCleanEvent += OnWindowClosed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 침대 치우기
        if (InputManager.Instance.GetKey_Timed(ConsoleKey.LeftArrow))
        {
            GameManager.Instance.CleanBed();
        }
        // 화분 치우기
        else if (InputManager.Instance.GetKey_Timed(ConsoleKey.RightArrow))
        {
            GameManager.Instance.CleanPlant();
        }
        // 창문 닫기
        else if (InputManager.Instance.GetKey_Timed(ConsoleKey.UpArrow))
        {
            GameManager.Instance.CloseWindow();
        }
    }

    public override void OnDestroy()
    {
        GameManager.Instance.BedCleanEvent -= OnBedCleaned;
        GameManager.Instance.PlantCleanEvent -= OnPlantCleaned;
        GameManager.Instance.WindowCleanEvent -= OnWindowClosed;

        base.OnDestroy();
    }

    void OnBedCleaned(object? sender, EventArgs e)
    {
        if (bedRenderer != null)
        {
            bedRenderer.CharSpriteKey = "cleanBed";
        }
    }

    void OnPlantCleaned(object? sender, EventArgs e)
    {
        if (plantRenderer != null)
        {
            plantRenderer.CharSpriteKey = "cleanPlant";
        }
    }

    void OnWindowClosed(object? sender, EventArgs e)
    {
        if (windowRenderer != null)
        {
            windowRenderer.CharSpriteKey = "cleanWindow";
        }
    }
}