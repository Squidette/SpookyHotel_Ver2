﻿/// <summary>
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

        /// 프론트맨 대사
        /*0*/ dialogues.Add("어서오게.\r\n원래 이 호텔은 귀신이 있는\r\n으스스한 곳이어야 하는데,\r\n개발자의 시간부족으로 귀신이\r\n없는 평범한 호텔이 되어\r\n버렸다네. 아쉽구먼.. ▶▶");
        /*1*/ dialogues.Add("아, 그리고 최근 44호에서\r\n불미스러운 일이 있었다네.\r\n지금 경찰이 조사 중이니\r\n그 방은 들어가지 말고\r\n나머지 방만 부탁하네.\r\n\r\n쨌든, 2층부터 7층까지\r\n청소해주고 끝나는대로\r\n일찍 가게나.");
        /*2*/ dialogues.Add("치울 필요 없는 물건은\r\n만지지 말게.\r\n부정탄다네.. 껄껄");

        /// 방 첫입장시 독백대사 + 청소 튜토리얼
        /*3*/dialogues.Add("방마다 침대, 화분, 창문을 모두\r\n청소하고 나가면 된다고 했다");
        /*4*/dialogues.Add("그런데 이미 깨끗한 건 손대지\r\n않는 게 나을 것 같다는 느낌이\r\n강하게 든다.\r\n불길하다.\r\n\r\n그래도 일단 이 방은 전부 더럽네.");
        /*5*/dialogues.Add("[←] 침대 치우기\r\n[→] 화분 치우기\r\n[↑] 창문 닦기\r\n\r\n[Space] 방 나가기");

        /// 생명 깎일떄마다 나오는 대사
        /*6*/dialogues.Add("더이상 손이 닿으면 큰일 날 것\r\n같다. 나가고 싶어."); //1
        /*7*/dialogues.Add("왜 아프지?"); //2
        /*8*/dialogues.Add("앗."); //3
        /*9*/dialogues.Add("만질 필요 없는 이미 깨끗한\r\n물건을 만져 버렸다. 조심."); //4

        /// 튜토리얼 안하고 404호 들어가려 하면 나오는 독백
        /*10*/dialogues.Add("여긴 들어가지 말라고 했나");

        /// 404호에 다시 들어가려고 하면 나오는 독백
        /*11*/dialogues.Add("열지 말걸 이놈의 호기심이\r\n문제다");

        /// 엘리베이터 튜토리얼
        /*12*/dialogues.Add("[1], [2], [3], [4], [5], [6]\r\n키로 가려는 층의 버튼 누르기");

        /// 첫입장 튜토리얼
        /*13*/dialogues.Add("\r\n[A],[D] 좌우 이동\r\n[Space] 문 출입, 텍스트 넘기기\r\n\r\n\r\n\r\n\r\n\r\n                       START >");

        /// 미친놈 대사1
        /*14*/dialogues.Add("행운의 숫자 7~\r\n행운의 숫자 7~\r\n\r\n행운의 숫자 7~\r\n랄라랄라라");

        /// 미친놈 대사2
        /*15*/dialogues.Add("네이이이이노오오오옴\r\n행운의 숫자 7의 힘을 거부하는\r\n것인가?\r\n띵똥띠로디로딩!");

        /// 엔딩 A
        /*16*/dialogues.Add("기분나쁜 호텔 탈출 성공!\r\n\r\n[엔딩 A] 역시 기분나쁜 것들은\r\n    애초에 손대지 않는 것이\r\n    좋지요");

        /// 엔딩 B
        /*17*/dialogues.Add("무한의 방에서 최후를 맞이하다\r\n\r\n[엔딩 B] 호기심의 값진 결과를\r\n    맞이했습니다");

        /// 엔딩 C
        /*18*/dialogues.Add("으아아아아아아아악!\r\n\r\n당신은 알 수 없는 이유로 죽었습\r\n니다.\r\n\r\n[엔딩 C] 마음이 너무 급하거나,\r\n    이 게임을 하기 귀찮아 보입\r\n    니다.");
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