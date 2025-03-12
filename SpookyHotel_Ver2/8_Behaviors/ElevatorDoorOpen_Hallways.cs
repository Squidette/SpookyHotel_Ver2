class ElevatorDoorOpen_Hallways : ElevatorDoorOpen
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnStoppedStateChanged(object? sender, Elevator.ElevatorDoorOpenEventArgs e)
    {
        bool display = true;

        // 현재 층이 아니면 문 열지않기
        if (e.open && GameManager.Instance.playerCurrentFloor != Elevator.Instance.CurrentFloor)
        {
            display = false;
        }

        if (display && doorOpenSprite != null)
        {
            doorOpenSprite.enabled = e.open;
        }
    }

    // 여닫힐 때 사운드
    protected override void OnReservedStateChanged(object? sender, Elevator.ElevatorDoorOpenEventArgs e)
    {
        if (GameManager.Instance.playerCurrentFloor == Elevator.Instance.CurrentFloor)
        {
            SoundManager.Instance.PlaySnippet(e.open ? "voice_opendoor.mp3" : "voice_closedoor.mp3");
        }
    }
}