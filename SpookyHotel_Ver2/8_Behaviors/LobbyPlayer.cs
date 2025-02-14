// 로비에서의 플레이어 행동
class LobbyPlayer : Behavior
{
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (EntersElevator())
        {
            SceneManager.Instance.LoadScene<ElevatorInside>();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    bool SpeaksToFront()
    {
        return gameObject.Transform.position.row > 35 && gameObject.Transform.position.row < 53 && InputManager.Instance.GetKey(ConsoleKey.Spacebar);
    }

    bool EntersElevator()
    {
        return gameObject.Transform.position.row > 65 && gameObject.Transform.position.row < 75 && InputManager.Instance.GetKey(ConsoleKey.Spacebar);
    }
}