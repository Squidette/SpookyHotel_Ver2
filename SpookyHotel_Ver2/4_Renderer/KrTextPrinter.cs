/// <summary>
/// 한국어 다이얼로그 출력을 위한 간단한 텍스트 프린터
/// 영어와 특수문자만 쓰는 기존의 콘솔렌더러와는 한국어 글자크기가 달라서 이렇게 따로 뺐다
/// </summary>
class KrTextPrinter
{
    static KrTextPrinter instance = null!;

    public static KrTextPrinter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new KrTextPrinter();
            }
            return instance;
        }
    }

    string? waitingText = null;

    /// <summary>
    /// 씬에서 텍스트 출력을 위해 부르는 함수
    /// </summary>
    /// <param name="text"></param>
    public void PrintText(string text)
    {
        waitingText = text;
    }

    /// <summary>
    /// 메인 루프에서 불릴 함수
    /// </summary>
    /// <param name="text"></param>
    public void Print()
    {
        if (waitingText != null)
        {
            Console.Clear();
            Console.Write(waitingText);
            waitingText = null;
        }
    }
}