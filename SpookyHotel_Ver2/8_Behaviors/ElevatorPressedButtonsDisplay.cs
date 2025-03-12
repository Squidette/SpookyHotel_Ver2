class ElevatorPressedButtonsDisplay : Behavior
{
    public GameObject[] pressedButtonDisplayers = null!;

    public override void Start()
    {
        Elevator.Instance.RaiseButtonPressEvent += OnButtonStateChanged;
        base.Start();
    }

    public override void OnDestroy()
    {
        Elevator.Instance.RaiseButtonPressEvent -= OnButtonStateChanged;
        base.OnDestroy();
    }

    void OnButtonStateChanged(object? sender, Elevator.PressedButtonEventArgs e)
    {
        if (pressedButtonDisplayers != null && pressedButtonDisplayers.Length >= e.floor)
        {
            GameObject displayer = pressedButtonDisplayers[e.floor - 1];

            if (displayer != null)
            {
                CharSpriteRenderer renderer = displayer.GetComponent<CharSpriteRenderer>();

                if (renderer != null)
                {
                    renderer.enabled = e.pressed;
                }
            }
        }

        // 사운드
        if (e.pressed)
        {
            switch (e.floor)
            {
                case 1:
                    SoundManager.Instance.PlaySnippet("voice_1f.mp3");
                    break;
                case 2:
                    SoundManager.Instance.PlaySnippet("voice_2f.mp3");
                    break;
                case 3:
                    SoundManager.Instance.PlaySnippet("voice_3f.mp3");
                    break;
                case 4:
                    SoundManager.Instance.PlaySnippet("voice_4f.mp3");
                    break;
                case 5:
                    SoundManager.Instance.PlaySnippet("voice_5f.mp3");
                    break;
                case 6:
                    SoundManager.Instance.PlaySnippet("voice_6f.mp3");
                    break;
                case 7:
                    SoundManager.Instance.PlaySnippet("voice_7f.mp3");
                    break;
            }
        }
    }
}
