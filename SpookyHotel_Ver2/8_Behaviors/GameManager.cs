class GameManager : Behavior
{
    static bool created = false;
    public static bool Created
    {
        get { return created; }
    }

    public GameManager()
    {
        // 유사 싱글톤
        created = true;

        // 게임 관련
        playerLife = 5;

        seventhFloorUnlocked = false;
        currentFloor = 1;
        direction = null;
    }

    /// <summary>
    /// 플레이어 관련
    /// </summary>

    // 생명
    int playerLife;

    /// <summary>
    /// 엘리베이터 관련
    /// </summary>
    
    // 최대 층 관련
    bool seventhFloorUnlocked;
    public bool SeventhFloorUnlocked
    {
        get { return seventhFloorUnlocked; }
        set { seventhFloorUnlocked = value; }
    }
    public int GetMaxFloor()
    {
        return seventhFloorUnlocked ? 7 : 6;
    }

    // 현재 층
    int currentFloor;
    public int CurrentFloor
    {
        get
        {
            return currentFloor;
        }
        set
        {
            currentFloor = value;
        }
    }

    // 진행 방향    // null: 멈춤, true: 위, false: 아래
    bool? direction; 
    public bool? Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    // 엘리베이터 눌린 버튼
    HashSet<int> CurrentPressedFloors = new HashSet<int>();
    public void AddToCurrentPressedFloors(int floor)
    {
        if (0 < floor && floor <= GetMaxFloor())
        {
            CurrentPressedFloors.Add(floor);
        }
    }
    void RemoveFromCurrentPressedFloors(int floor)
    {
        CurrentPressedFloors.Remove(floor);
    }

    // 버튼 누르기
    public void PressElevatorButton(int floor)
    {
        if (0 < floor && floor <= GetMaxFloor())
        {

        }
    }

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}