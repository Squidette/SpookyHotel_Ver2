class Player : Behavior
{
    static bool created = false;
    public static bool Created
    {
        get { return created; }
    }

    public Player()
    {
        created = true;
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    protected override void FixedUpdate()
    {
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