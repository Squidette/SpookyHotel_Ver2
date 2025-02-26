/// <summary>
/// Console.ReadKey를 사용한 간단한 인풋매니저
/// 동시 인풋은 받을 수 없다
/// </summary>
class InputManager
{
    // 싱글턴
    static InputManager instance = null!;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }

    // 마지막으로 눌린 키
    ConsoleKey lastPressedKey;
    public ConsoleKey CurrentPressedKey { get { return lastPressedKey; } }

    // Console.KeyAvailable이 생각보다 자주 true가 되지 않아서, 키가 눌렸을 때 일정하게 true를 리턴해주기 위한 장치 추가
    bool keyHeld = false;

    // TimedGetKey 시간재기용
    FCTimer timer;

    //
    List<ConsoleKey> timedKeys;
    
    InputManager()
    {
        ResetPressedKey();

        timedKeys = new List<ConsoleKey>();
        timedKeys.Add(ConsoleKey.Spacebar);

        timer = new FCTimer(10, true);
    }

    public void Update()
    {
        if (Console.KeyAvailable) 
        {
            lastPressedKey = Console.ReadKey(intercept: true).Key;
            keyHeld = true;
        }
    }

    public void FixedUpdate()
    {
        if (!keyHeld) ResetPressedKey();
        keyHeld = false;
    }

    public void ResetPressedKey()
    {
        lastPressedKey = ConsoleKey.None;
    }

    /// <summary>
    /// 다른 스크립트의 FixedUpdate에서 사용
    /// </summary>
    /// <param name="consoleKey"></param>
    /// <returns>키가 눌렸는가?</returns>
    public bool GetKey(ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.None) return false;

        bool pressed = consoleKey == lastPressedKey;
        if (pressed && timedKeys.Contains(consoleKey)) timer.Reset();

        return pressed;
    }

    /// <summary>
    /// GetKeyDown 만들기에 실패해서 일단 이렇게
    /// 마지막으로 키를 누른 지 일정시간이 지나야 true를 반환해줌. 연속 클릭이 되는걸 방지할때 쓰자
    /// </summary>
    /// <param name="consoleKey"></param>
    /// <returns></returns>
    public bool GetKey_Timed(ConsoleKey consoleKey)
    {
        return timer.Past ? GetKey(consoleKey) : false;
    }
}