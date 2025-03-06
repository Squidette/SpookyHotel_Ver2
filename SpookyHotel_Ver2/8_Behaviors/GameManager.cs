/// <summary>
/// 게임진행 필요한 정보를 관리
/// </summary>
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

        // 진행 관련
        talkedToFrontMan = false;
        calledElevator = false;
        lunaticDialogueStage = 0;

        InstantiateRoomInfo();

        playerCurrentFloor = 1;
        playerCurrentRoomNum = -1;

        roomsLeftToClean = 30;
        hallwaysPosition = 58;

        enteredRoom = false;

        // 게임 관련
        playerLife = 5;
    }

    /// 건물관련

    // 플레이어 현재 층
    public int playerCurrentFloor; // 1~7

    // 로비 포지션
    public int lobbyPosition;

    // 복도 포지션
    public int hallwaysPosition;

    // 호실 정보
    public struct RoomInfo
    {
        bool bedClean;
        public bool BedClean
        {
            get => bedClean;
            set
            {
                if (value)
                {
                    if (bedClean) Instance.ReducePlayerLife();
                    else Instance.BedCleanEvent?.Invoke(this, new EventArgs());
                }
                bedClean = value;
            }
        }
        bool plantClean;
        public bool PlantClean
        {
            get => plantClean;
            set
            {
                if (value)
                {
                    if (plantClean) Instance.ReducePlayerLife();
                    else Instance.PlantCleanEvent?.Invoke(this, new EventArgs());
                }
                plantClean = value;
            }
        }
        bool windowClean;
        public bool WindowClean
        {
            get => windowClean;
            set
            {
                if (value)
                {
                    if (windowClean) Instance.ReducePlayerLife();
                    else Instance.WindowCleanEvent?.Invoke(this, new EventArgs());
                }
                windowClean = value;
            }
        }

        public RoomInfo()
        {
            Random myRandom = new Random();
            bedClean = myRandom.Next(0, 100) >= bedDirtyProbability;
            plantClean = myRandom.Next(0, 100) >= plantDirtyProbability;
            windowClean = myRandom.Next(0, 100) >= windowDirtyProbability;
        }

        // 이 방이 깨끗한지
        public bool Clean()
        {
            return bedClean && plantClean && windowClean;
        }

        public void SetAllDirty()
        {
            bedClean = false;
            plantClean = false;
            windowClean = false;
        }

        public void SetAllClean()
        {
            bedClean = true;
            plantClean = true;
            windowClean = true;
        }
    }

    RoomInfo[,] roomInfo = null!; // [층, 호수]
    public ref RoomInfo GetCurrentRoomInfo() => ref roomInfo[playerCurrentFloor - 2, playerCurrentRoomNum - 1];
    public event EventHandler<EventArgs>? BedCleanEvent;
    public event EventHandler<EventArgs>? PlantCleanEvent;
    public event EventHandler<EventArgs>? WindowCleanEvent;
    public void CleanBed() => roomInfo[playerCurrentFloor - 2, playerCurrentRoomNum - 1].BedClean = true;
    public void CleanPlant() => roomInfo[playerCurrentFloor - 2, playerCurrentRoomNum - 1].PlantClean = true;
    public void CloseWindow() => roomInfo[playerCurrentFloor - 2, playerCurrentRoomNum - 1].WindowClean = true;


    // 방 초기화 관련. 확률 변수로 0~100 범위
    const int bedDirtyProbability = 85;      // 침대가 더러울 확률 85?
    const int windowDirtyProbability = 40;    // 창문이 열려있을 확률 40?
    const int plantDirtyProbability = 25;    // 화분이 더러울(엎어져있을) 확률 25?

    // 플레이어 현재 호실
    public int playerCurrentRoomNum;

    /// 플레이어 관련
    
    // 생명
    int playerLife;
    public int PlayerLife { get => playerLife; }
    void ReducePlayerLife()
    {
        playerLife--;

        if (playerLife <= 4 && playerLife >= 1) SceneManager.Instance.LoadScene<LifeMonologue>();
        else if (playerLife <= 0)
        {
            Debug.Log("죽었습니다");
        }
    }

    // 치울 방 개수
    public event EventHandler<EventArgs>? RaiseRoomsLeftToCleanChanged;
    int roomsLeftToClean;
    public int RoomsLeftToClean
    {
        get => roomsLeftToClean;
        set
        {
            if (roomsLeftToClean != value)
            {
                roomsLeftToClean = value;
            }
        }
    }

    /// 진행 관련

    // 프론트맨에게 말을 걸었는지 여부
    public bool talkedToFrontMan;

    // 미친놈의 대사 진행도
    public int lunaticDialogueStage;

    // 처음으로 방에 들어갔는지 여부
    bool enteredRoom;
    public bool EnteredRoom
    {
        get => enteredRoom;
        set
        {
            if (value != enteredRoom)
            {
                enteredRoom = value;

                // 처음 입장시 튜토리얼 재생
                if (enteredRoom)
                {
                    // 튜토리얼이므로 현재 방의 모든 기물은 Dirty로 만들어준다
                    GetCurrentRoomInfo().SetAllDirty();

                    // 튜토씬 띄우기
                    SceneManager.Instance.LoadScene<RoomCleanTutorial>();
                }
            }
        }
    }

    // 엘리베이터 입장 시도했는지 여부 (아니면 튜토리얼 켜려고)
    public bool calledElevator;

    public override void Start()
    {
        base.Start();

        SceneManager.Instance.DontDestroyOnLoad(gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    void InstantiateRoomInfo()
    {
        roomInfo = new RoomInfo[5, 6];
        ArrayUtils.Iterate(roomInfo, (i, j) => roomInfo[i, j] = new RoomInfo());

        // 404호는 일반 룸 취급하지 않으므로 전부 Clean으로 만들어주고 무시한다
        roomInfo[2, 3].SetAllClean();
    }

    public void CountUpCleanRooms()
    {
        int dirtyRooms = 0;

        ArrayUtils.Iterate(roomInfo, (i, j) =>
        {
            if (!roomInfo[i, j].Clean()) dirtyRooms++;
        });

        RoomsLeftToClean = dirtyRooms;
        Instance.RaiseRoomsLeftToCleanChanged?.Invoke(this, new EventArgs());
    }
}