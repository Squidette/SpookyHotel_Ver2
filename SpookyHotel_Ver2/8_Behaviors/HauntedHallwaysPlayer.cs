class HauntedHallwaysPlayer : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (InputManager.Instance.GetKey_Timed(ConsoleKey.Spacebar))
        {
            // 엘리베이터 입장하기
            if (gameObject.Transform.position.row >= 56 && gameObject.Transform.position.row <= 64)
            {
                SoundManager.Instance.PlaySnippet("voice_nofloor.mp3");
            }
            else
            {
                SceneManager.Instance.LoadScene<Ending2>();
            }
        }
    }
}