class CameraMove : Behavior
{
    // 따라다닐 게임오브젝트
    public GameObject? target;

    public bool limitLess = false;

    // 리밋
    public int minRow;
    public int maxRow;

    // C000000000
    // 0000000000
    // 0000000000
    // 0000000000

    // 카메라는 위 주석처럼 중점에서 오른쪽 아래부분을 프린트하기때문에 적당한 오프셋설정이 필요
    public CharSpriteCoords offset = new CharSpriteCoords(-5, -14);

    public override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 타겟이 있으면 따라다닌다
        if (target != null)
        {
            gameObject.Transform.position = target.Transform.position + offset;

            if (!limitLess)
            {
                if (gameObject.Transform.position.row < minRow) gameObject.Transform.position.row = minRow;
                if (gameObject.Transform.position.row > maxRow) gameObject.Transform.position.row = maxRow;
            }
        }

        //if (InputManager.Instance.GetKey(ConsoleKey.P))
        //{
        //    Debug.Log(gameObject.Transform.position.ToString());
        //}
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}