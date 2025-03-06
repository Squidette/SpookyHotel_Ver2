/// <summary>
/// 다이얼로그용 씬으로 반드시 상속하여 사용해야 하고, dialogueIndices, LoadNextScene의 내용을 채워줘야한다
/// </summary>
abstract class DialogueScene : Scene
{
    /// <summary>
    /// 다이얼로그 인덱스는 KrTextPrinter에서 찾을 수 있다
    /// </summary>
    protected Queue<int> dialogueIndices;

    public DialogueScene()
    {
        dialogueIndices = new Queue<int>();
    }

    public override void Start()
    {
        base.Start();

        ShowNextDialogue();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
        {
            ShowNextDialogue();
        }
    }

    public override void Exit()
    {
        SpookyHotel_Ver2.renderMode = SpookyHotel_Ver2.RenderMode.CONSOLERENDERER;
        base.Exit();
    }

    // 다음 다이얼로그로 넘어가기
    void ShowNextDialogue()
    {
        if (dialogueIndices.Count > 0)
        {
            SpookyHotel_Ver2.renderMode = SpookyHotel_Ver2.RenderMode.KRTEXTPRINTER;
            KrTextPrinter.Instance.PrintText(dialogueIndices.Dequeue());
        }
        else // 다음 다이얼로그가 없으면 씬 나가기
        {
            LoadNextScene();
        }
    }

    /// <summary>
    /// 이 대화가 끝나면 어떤 씬으로 나갈 것인지 지정
    /// </summary>
    protected abstract void LoadNextScene();
}

/// <summary>
/// 개별 다이얼로그들
/// </summary>
class HotelFrontManDialogue : DialogueScene
{
    public HotelFrontManDialogue() : base()
    {
        dialogueIndices.Enqueue(0);
        dialogueIndices.Enqueue(1);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Lobby>();
    }
}

class HotelFrontManComments : DialogueScene
{
    public HotelFrontManComments() : base()
    {
        dialogueIndices.Enqueue(2);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Lobby>();
    }
}

class LifeMonologue : DialogueScene
{
    public LifeMonologue() : base()
    {
        if (GameManager.Instance != null)
        {
            dialogueIndices.Enqueue(GameManager.Instance.PlayerLife + 5);
        }
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Room>();
    }
}

class RoomCleanTutorial : DialogueScene
{
    public RoomCleanTutorial() : base()
    {
        dialogueIndices.Enqueue(3);
        dialogueIndices.Enqueue(4);
        dialogueIndices.Enqueue(5);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Room>();
    }
}

class HauntedRoomDenial : DialogueScene
{
    public HauntedRoomDenial() : base()
    {
        dialogueIndices.Enqueue(10);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Hallways>();
    }
}

class HauntedRoomAfterMonologue : DialogueScene
{
    public HauntedRoomAfterMonologue() : base()
    {
        dialogueIndices.Enqueue(11);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Hallways>();
    }
}

class ElevatorTutorial : DialogueScene
{
    public ElevatorTutorial() : base()
    {
        dialogueIndices.Enqueue(12);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<ElevatorInside>();
    }
}

class FirstTutorial : DialogueScene
{
    public FirstTutorial() : base()
    {
        dialogueIndices.Enqueue(13);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Lobby>();
    }
}

class LunaticDialogue1 : DialogueScene
{
    public LunaticDialogue1() : base()
    {
        dialogueIndices.Enqueue(14);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<HauntedRoom>();
    }
}

class LunaticDialogue2 : DialogueScene
{
    public LunaticDialogue2() : base()
    {
        dialogueIndices.Enqueue(15);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<HauntedRoom>();
    }
}