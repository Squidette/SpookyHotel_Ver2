class Player : Behavior
{
    static bool created = false;
    public static bool Created
    {
        get { return created; }
    }

    // 최대/최소 움직임 좌표 (리밋)
    int minRow;
    public int MinRow { set { minRow = value; } }
    int maxRow;
    public int MaxRow { set { maxRow = value; } }

    public Player()
    {
        created = true;
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    public override void FixedUpdate()
    {
        if (InputManager.Instance.GetKey(ConsoleKey.A) && gameObject.Transform.position.row > minRow)
        {
            gameObject.Transform.position.row = gameObject.Transform.position.row - 1;
        }
        else if (InputManager.Instance.GetKey(ConsoleKey.D) && gameObject.Transform.position.row < maxRow)
        {
            gameObject.Transform.position.row = gameObject.Transform.position.row + 1;
        }

        // debug
        if (InputManager.Instance.GetKey(ConsoleKey.I))
        {
            Debug.Log("Player Position: " + gameObject.Transform.position.ToString());
        }

        base.FixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}