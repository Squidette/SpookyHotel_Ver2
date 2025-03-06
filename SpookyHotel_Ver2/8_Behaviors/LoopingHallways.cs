/// <summary>
/// 무한 배경 생성기. 3개의 배경을 쳇바퀴처럼 돌려쓴다
/// </summary>
class LoopingHallways : Behavior
{
    public Transform player = null!;

    // 쳇바퀴처럼 도는 3개의 배경 오브젝트 트랜스폼
    Transform[] slicedBackgrounds = null!;

    // 어떤 인덱스의 오브젝트가 가장 왼쪽에 있는지
    // 0 : 0 1 2
    // 1 : 1 2 0 <- 이 순서는 transform.position을 시각화한것
    // 2 : 2 0 1
    int slicesOrder = 0;

    // 배경들 사이의 거리
    const int distanceBetween = 16;

    // 가장 왼쪽 배경의 좌표
    ref int GetLeftPos() { return ref slicedBackgrounds[slicesOrder].position.row; }
    // 가장 오른쪽 배경의 좌표
    ref int GetRightPos() { return ref slicedBackgrounds[(slicesOrder + 2) % 3].position.row; }

    bool playerNearElevator = true;

    // 3개의 배경 오브젝트 지정
    public void SetThreeLoopingBackgroundObjects(GameObject loop1, GameObject loop2, GameObject loop3)
    {
        slicedBackgrounds[0] = loop1.Transform;
        slicedBackgrounds[1] = loop2.Transform;
        slicedBackgrounds[2] = loop3.Transform;
    }

    // 엘리베이터 근처에 차례대로 세팅(오른쪽으로 가려할때와 왼쪽으로 가려할때가 다르다)
    public void ResetBackgroundInitialOffests(bool right)
    {
        if (right)
        {
            slicedBackgrounds[0].position.row = 86;
            slicedBackgrounds[1].position.row = 102;
            slicedBackgrounds[2].position.row = 118;
        }
        else
        {
            slicedBackgrounds[0].position.row = -13;
            slicedBackgrounds[1].position.row = 3;
            slicedBackgrounds[2].position.row = 19;
        }

        slicesOrder = 1;
    }

    // 오른쪽으로 굴리기
    public void IncreaseSlicesOrder()
    {
        // 가장 오른쪽에 위치한 오브젝트의 인덱스
        int rightest = (slicesOrder + 2) % 3;

        // 맨 왼쪽에 있는걸 가장 오른쪽에있는애 오른쪽으로 옮기기
        //slicedBackgrounds[slicesOrder].position.row = slicedBackgrounds[rightest].position.row + distanceBetween;
        GetLeftPos() = GetRightPos() + distanceBetween;

        // 현 상태변수 증가시키기
        slicesOrder = (slicesOrder + 1) % 3;
    }

    // 왼쪽으로 굴리기
    public void DecreaseSlicesOrder()
    {
        // 가장 오른쪽에 위치한 오브젝트의 인덱스
        int rightest = (slicesOrder + 2) % 3;

        // 맨 오른쪽에 있는 배경을 가장 왼쪽에있는애 왼쪽으로 옮기기
        //slicedBackgrounds[rightest].position.row = slicedBackgrounds[slicesOrder].position.row - distanceBetween;
        GetRightPos() = GetLeftPos() - distanceBetween;

        // 현 상태변수 감소시키기
        slicesOrder = (slicesOrder + 2) % 3;
    }

    public LoopingHallways()
    {
        slicedBackgrounds = new Transform[3];
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        /// 플레이어가 오른쪽으로 걸어간다면
        if (player.position.row >= 102)
        {
            // 처음 벗어났다면 배경을 오른쪽에 리셋
            if (playerNearElevator)
            {
                ResetBackgroundInitialOffests(true);
                playerNearElevator = false;
            }

            // 계속 나아간다면 배경 굴리기
            LoopBackgrounds();
        }
        /// 플레이어가 왼쪽으로 걸어간다면
        else if (player.position.row < 25)
        {
            // 처음 벗어났다면 배경을 왼쪽에 리셋
            if (playerNearElevator)
            {
                ResetBackgroundInitialOffests(false);
                playerNearElevator = false;
            }

            // 계속 나아간다면 배경 굴리기
            LoopBackgrounds();
        }
        // 엘리베이터 부근에서는 bool값 다시 true로 만들어주기
        else playerNearElevator = true;
    }

    void LoopBackgrounds()
    {
        // 플레이어가 우측 범위에서 벗어났다면 오른쪽으로 한칸 굴리기
        if (player.position.row >= GetRightPos()) { IncreaseSlicesOrder(); }
        // 플레이어가 좌측 범위에서 벗어났다면 왼쪽으로 한칸 굴리기
        else if (player.position.row <= GetLeftPos() + 14) { DecreaseSlicesOrder(); }
    }
}