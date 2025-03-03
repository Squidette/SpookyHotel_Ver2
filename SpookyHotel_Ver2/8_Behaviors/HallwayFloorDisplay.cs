class HallwayFloorDisplay : Behavior
{
    public CharRenderer charRenderer = null!;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (charRenderer != null)
        {
            charRenderer.character = (char)('0' + Elevator.Instance.CurrentFloor);
        }
    }
}