class RoomsLeftToCleanUI : Behavior
{
    CharSprite roomsLeftToCleanCharSprite = null!;
    public CharSpriteRenderer renderer = null!;

    public override void Start()
    {
        base.Start();

        roomsLeftToCleanCharSprite = ConsoleRenderer.Instance.GetSprite("roomsLeftToClean")!;
        GameManager.Instance.RaiseRoomsLeftToCleanChanged += OnRoomsLeftChanged;
    }

    void OnRoomsLeftChanged(object? sender, EventArgs e)
    {
        if (roomsLeftToCleanCharSprite != null)
        {
            string str = GameManager.Instance.RoomsLeftToClean.ToString().PadLeft(2, ' ');
            roomsLeftToCleanCharSprite.SetCharByCoords(new CharSpriteCoords(0, 0), str[0]);
            roomsLeftToCleanCharSprite.SetCharByCoords(new CharSpriteCoords(0, 1), str[1]);
        }
    }

    public override void OnDestroy()
    {
        GameManager.Instance.RaiseRoomsLeftToCleanChanged -= OnRoomsLeftChanged;

        base.OnDestroy();
    }
}