static class Debug
{
    static int currentLine = 0;

    // 버퍼 넘어가면 에러 떠서 줄 넘어가면 0줄부터 덮어쓰게 했다
    static public void Log(string message)
    {
        if (currentLine + 10 >= Console.BufferHeight - 1) currentLine = 0;
        Console.SetCursorPosition(0, currentLine + 10);
        Console.WriteLine(message);
        currentLine++;
    }
}