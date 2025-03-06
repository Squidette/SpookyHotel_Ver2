class LunaticSpeak : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
        {
            switch (GameManager.Instance.lunaticDialogueStage)
            {
                case 0:
                    SceneManager.Instance.LoadScene<LunaticDialogue1>();
                    GameManager.Instance.lunaticDialogueStage++;
                    break;
                case 1:
                    SceneManager.Instance.LoadScene<LunaticDialogue2>();
                    GameManager.Instance.lunaticDialogueStage++;
                    break;
                default:
                    SceneManager.Instance.LoadScene<Hallways>();
                    break;
            }
        }
    }
}