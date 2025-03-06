class HallwayFloorDisplay : Behavior
{
    public CharRenderer charRenderer = null!;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (charRenderer != null)
        {
            if (Elevator.Instance.CurrentFloor == 1)
            {
                charRenderer.character = 'L';
            }
            else
            {
                charRenderer.character = (char)('0' + Elevator.Instance.CurrentFloor);
            }
        }
    }
}