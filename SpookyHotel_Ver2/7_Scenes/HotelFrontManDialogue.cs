class HotelFrontManDialogue : DialogueScene
{
    public HotelFrontManDialogue()
        : base()
    {
        dialogueIndices.Enqueue(0);
        dialogueIndices.Enqueue(1);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Lobby>();
    }
}