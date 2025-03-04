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
        }
    }
}