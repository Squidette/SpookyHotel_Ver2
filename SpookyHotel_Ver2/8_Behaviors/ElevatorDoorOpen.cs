class ElevatorDoorOpen : Behavior
{
    public CharSpriteRenderer doorOpenSprite = null!;

    public override void Start()
    {
        Elevator.Instance.RaiseStoppedEvent += OnStoppedStateChanged;
        base.Start();
    }

    public override void OnDestroy()
    {
        Elevator.Instance.RaiseStoppedEvent -= OnStoppedStateChanged;
        base.OnDestroy();
    }

    void OnStoppedStateChanged(object? sender, Elevator.ElevatorStoppedEventArgs e)
    {
        if (doorOpenSprite != null)
        {
            doorOpenSprite.enabled = e.stopped;
        }
    }
}