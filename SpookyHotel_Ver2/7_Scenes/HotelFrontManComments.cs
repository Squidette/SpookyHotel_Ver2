class HotelFrontManComments : DialogueScene
{
    public HotelFrontManComments()
        : base()
    {
        dialogueIndices.Enqueue(2);
    }

    protected override void LoadNextScene()
    {
        SceneManager.Instance.LoadScene<Lobby>();
    }
}