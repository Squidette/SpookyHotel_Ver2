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
    }
}
