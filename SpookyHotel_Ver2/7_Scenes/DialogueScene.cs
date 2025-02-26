/// <summary>
/// 다이얼로그용 씬으로 반드시 상속하여 사용해야 하고, dialogueIndices, LoadNextScene의 내용을 채워줘야한다
/// </summary>
abstract class DialogueScene : Scene
{
    // 이 다이얼로그 씬에서 보여줄 다이얼로그의 큐
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