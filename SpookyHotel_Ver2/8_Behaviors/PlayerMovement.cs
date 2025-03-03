class PlayerMovement : Behavior
{
    CharSpriteRenderer spriteRenderer = null!;

    // 최대/최소 움직임 좌표 (리밋)
    int minRow;
    public int MinRow { set { minRow = value; } }
    int maxRow;
    public int MaxRow { set { maxRow = value; } }

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

    public override void Start()
    {
        base.Start();

        spriteRenderer = gameObject.GetComponent<CharSpriteRenderer>();
        lastAnimChangePosition = gameObject.Transform.position.row;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 이동키 받음
        if (InputManager.Instance.GetKey(ConsoleKey.A) && gameObject.Transform.position.row > minRow)
        {
            gameObject.Transform.position.row = gameObject.Transform.position.row - 1;
            staticCombo = 0;
        }
        else if (InputManager.Instance.GetKey(ConsoleKey.D) && gameObject.Transform.position.row < maxRow)
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
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}