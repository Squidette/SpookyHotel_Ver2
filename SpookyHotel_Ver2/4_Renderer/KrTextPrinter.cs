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

    List<string> dialogues;

    KrTextPrinter()
    {
        dialogues = new List<string>();
        dialogues.Clear();

        /*0*/ dialogues.Add("어서오게.\n원래 이 호텔은 귀신이 있는\n으스스한 곳이어야 하는데,\n개발자의 시간부족으로 귀신이\n없는 평범한 호텔이 되어\n버렸다네. 아쉽구먼.. ▶▶");
        /*1*/ dialogues.Add("아, 그리고 최근 44호에서\n불미스러운 일이 있었다네.\n지금 경찰이 조사 중이니\n그 방은 들어가지 말고\n나머지 방만 부탁하네.\n\n쨌든, 2층부터 7층까지\n청소해주고 끝나는대로\n일찍 가게나.");
        /*2*/ dialogues.Add("치울 필요 없는 물건은\n만지지 말게.\n부정탄다네.. 껄껄");
    }

    /// <summary>
    /// 씬에서 텍스트 출력을 위해 부르는 함수
    /// </summary>
    /// <param name="text"></param>
    public void PrintText(int dialogueIndex)
    {
        waitingText = dialogues[dialogueIndex];
        Debug.Log(dialogueIndex.ToString() + " is waiting");
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