/// <summary>
/// FCTimer = FixedUpdate Counts Timer
/// 몇 번의 FixedUpdate가 지나갔는지 카운트하는 타이머
/// </summary>
class FCTimer
{
    static List<FCTimer> myTimers = new List<FCTimer>();

    int count;
    readonly int frames;

    bool period = false;
    /// <summary>
    /// 입력한 주기가 돌아올때마다 true를 리턴
    /// </summary>
    public bool Period
    {
        get { return period; }
    }

    /// <summary>
    /// 입력한 시간(프레임 수)이 지났는지를 리턴
    /// </summary>
    bool past = false;
    public bool Past
    {
        get { return past; }
    }

    /// <summary>
    /// 몇 번의 FixedUpdate를 기다릴 것인가
    /// </summary>
    public FCTimer(int frames, bool startAsPassed = false)
    {
        count = 0;

        past = startAsPassed;

        if (frames < 0) this.frames = 0;
        else this.frames = frames;

        myTimers.Add(this);
    }

    void CountUp()
    {
        count++;

        if (frames <= count)
        {
            count = 0;
            period = true;
            past = true;
        }
        else
        {
            period = false;
        }
    }

    public void Reset()
    {
        count = 0;
        past = false;
    }

    public static void FixedUpdate()
    {
        foreach (FCTimer timer in myTimers)
        {
            timer.CountUp();
        }
    }

    /// <summary>
    /// 더 이상 쓰지 않는 타이머는 해제하기
    /// </summary>
    public static void ReleaseTimer(FCTimer timer)
    {
        if (timer == null) return;

        if (myTimers.Contains(timer))
        {
            myTimers.Remove(timer);
            timer.period = false;
        }
    }
}