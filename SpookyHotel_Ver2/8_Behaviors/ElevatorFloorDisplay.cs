class ElevatorFloorDisplay : Behavior
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Elevator.Instance != null)
        {
            //Elevator.Instance.CurrentFloor;

            //gameObject.Transform
            gameObject.Transform.position.row = Elevator.Instance.CurrentFloor * 4 - 1;
        }
    }
}