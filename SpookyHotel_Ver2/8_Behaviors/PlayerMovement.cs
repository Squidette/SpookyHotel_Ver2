class PlayerMovement : Behavior
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

    // 플레이어 조작 가능
    public bool canMove;

    // 걷기 애니메이션
    int animationSpeed = 3;     // 이 숫자의 fixedUpdate 주기마다 애니메이션을 바꾸게된다
    int staticCombo = 1000;     // 몇 FixeUpdate만큼 서있었는지? 초기값은 충분히 큰 수
    int lastAnimChangePosition;
    bool walkingAnimation;      // WalkingAnimation으로 접근에서 스프라이트 설정
    public bool WalkingAnimation
    {
        get
        {
            return walkingAnimation;
        }
        set
        {
            if (walkingAnimation != value)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.CharSpriteKey = value ? "player1" : "player2";
                }
                walkingAnimation = value;
            }
        }
    }
    CharSpriteRenderer spriteRenderer = null!;

    public PlayerMovement()
    {
        created = true;
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);

        spriteRenderer = gameObject.GetComponent<CharSpriteRenderer>();

        lastAnimChangePosition = gameObject.Transform.position.row;

        canMove = true;
    }

    public override void FixedUpdate()
    {
        // 이동키 받음
        if (InputManager.Instance.GetKey(ConsoleKey.A) && gameObject.Transform.position.row > minRow && canMove)
        {
            gameObject.Transform.position.row = gameObject.Transform.position.row - 1;
            staticCombo = 0;
        }
        else if (InputManager.Instance.GetKey(ConsoleKey.D) && gameObject.Transform.position.row < maxRow && canMove)
        {
            gameObject.Transform.position.row = gameObject.Transform.position.row + 1;
            staticCombo = 0;
        }
        else //서있었던 시간이 animationSpeed보다 커질 때 서 있는 모션으로 바꿔 준다
        {
            if (staticCombo < animationSpeed + 2) staticCombo++;
            if (staticCombo == animationSpeed + 1) WalkingAnimation = false;
        }

        // 걸은 거리에 따라 애니메이션을 토글해준다
        if (MathF.Abs(gameObject.Transform.position.row - lastAnimChangePosition) > animationSpeed)
        {
            WalkingAnimation = !walkingAnimation;
            lastAnimChangePosition = gameObject.Transform.position.row;
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

