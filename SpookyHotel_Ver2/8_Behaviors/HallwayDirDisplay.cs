class HallwayDirDisplay : Behavior
{
    public CharRenderer charRenderer = null!;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (charRenderer != null && Elevator.Instance != null)
        {
            switch (Elevator.Instance.Direction)
            {
                case null:
                    charRenderer.character = '-';
                    break;
                case true:
                    charRenderer.character = '↑';
                    break;
                case false:
                    charRenderer.character = '↓';
                    break;
            }
        }
    }
}