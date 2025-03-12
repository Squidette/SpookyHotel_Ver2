class ElevatorDoorOpen : Behavior
{
    public CharSpriteRenderer doorOpenSprite = null!;

    public override void Start()
    {
        Elevator.Instance.RaiseDoorEvent += OnStoppedStateChanged;
        Elevator.Instance.RaiseDoorReservedEvent += OnReservedStateChanged;
        base.Start();
    }

    public override void OnDestroy()
    {
        Elevator.Instance.RaiseDoorEvent -= OnStoppedStateChanged;
        Elevator.Instance.RaiseDoorReservedEvent -= OnReservedStateChanged;
        base.OnDestroy();
    }

    // 문이 여닫히는 비주얼
    protected virtual void OnStoppedStateChanged(object? sender, Elevator.ElevatorDoorOpenEventArgs e)
    {
        if (doorOpenSprite != null)
        {
            doorOpenSprite.enabled = e.open;
        }
    }

    // 여닫힐 때 사운드
    protected virtual void OnReservedStateChanged(object? sender, Elevator.ElevatorDoorOpenEventArgs e)
    {
        SoundManager.Instance.PlaySnippet(e.open ? "voice_opendoor.mp3" : "voice_closedoor.mp3");
    }
}