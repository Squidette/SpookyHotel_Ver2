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

    InputManager()
    {
        ResetPressedKey();
    }

    public void Update()
    {
        if (Console.KeyAvailable)
        {
            lastPressedKey = Console.ReadKey(intercept: true).Key;
        }
    }

    public void FixedUpdate()
    {
        ResetPressedKey();
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
        return consoleKey == lastPressedKey;
    }
}