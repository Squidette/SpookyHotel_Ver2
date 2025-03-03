class ElevatorDoorOpen : Behavior
{
    public CharSpriteRenderer doorOpenSprite = null!;

    public override void Start()
    {
        Elevator.Instance.RaiseDoorEvent += OnStoppedStateChanged;
        base.Start();
    }

    public override void OnDestroy()
    {
        Elevator.Instance.RaiseDoorEvent -= OnStoppedStateChanged;
        base.OnDestroy();
    }

    protected virtual void OnStoppedStateChanged(object? sender, Elevator.ElevatorDoorOpenEventArgs e)
    {
        if (doorOpenSprite != null)
        {
            doorOpenSprite.enabled = e.open;
        }
    }
}