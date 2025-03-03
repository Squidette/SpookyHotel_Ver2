class GameManager : Behavior
{
    static bool created = false;
    public static bool Created
    {
        get { return created; }
    }

    static GameManager instance = null!;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public GameManager()
    {
        // 유사 싱글톤 - 얘도 컴포넌트라서 얘만 따로 생성자를 숨길 방법을 못찾았다. 쓰는 사람이 싱글톤처럼 쓰도록..
        created = true;
        instance = this;

        // 대사 관련
        talkedToFrontMan = false;

        // 게임 관련
        //playerLife = 5;
    }

    /// <summary>
    /// 플레이어 관련
    /// </summary>

    // 플레이어 현재 층
    public int playerCurrentFloor;

    // 생명
    //int playerLife;

    /// <summary>
    /// 대사 관련
    /// </summary>
    
    // 프론트맨에게 말을 걸었는지 여부
    public bool talkedToFrontMan;

    // 로비 포지션
    public int lobbyPosition;

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);

        playerCurrentFloor = 1;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}