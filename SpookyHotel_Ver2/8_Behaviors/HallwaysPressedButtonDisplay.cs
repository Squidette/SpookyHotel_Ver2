class HallwaysPressedButtonsDisplay : Behavior
{
    CharSpriteRenderer renderer = null!;

    public override void Start()
    {
        base.Start();

        renderer = gameObject.GetComponent<CharSpriteRenderer>();
        Elevator.Instance.RaiseButtonPressEvent += OnButtonStateChanged;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Elevator.Instance.RaiseButtonPressEvent -= OnButtonStateChanged;
    }

    void OnButtonStateChanged(object? sender, Elevator.PressedButtonEventArgs e)
    {
        // 현재 층 신호가 보내지면
        if (GameManager.Instance != null && e.floor == GameManager.Instance.playerCurrentFloor)
        {
            if (renderer != null)
            {
                renderer.enabled = e.pressed;
            }

            //// 사운드
            //if (e.pressed)
            //{
            //    switch (e.floor)
            //    {
            //        case 1:
            //            SoundManager.Instance.PlaySnippet("voice_1f.mp3");
            //            break;
            //        case 2:
            //            SoundManager.Instance.PlaySnippet("voice_2f.mp3");
            //            break;
            //        case 3:
            //            SoundManager.Instance.PlaySnippet("voice_3f.mp3");
            //            break;
            //        case 4:
            //            SoundManager.Instance.PlaySnippet("voice_4f.mp3");
            //            break;
            //        case 5:
            //            SoundManager.Instance.PlaySnippet("voice_5f.mp3");
            //            break;
            //        case 6:
            //            SoundManager.Instance.PlaySnippet("voice_6f.mp3");
            //            break;
            //        case 7:
            //            SoundManager.Instance.PlaySnippet("voice_7f.mp3");
            //            break;
            //    }
            //}
        }
    }
}