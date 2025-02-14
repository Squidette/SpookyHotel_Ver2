class Transform : Component
{
    // 이 콘솔프로젝트에는 일단 스케일이나 로테이션같은건 없다!
    public CharSpriteCoords position;

    public Transform(CharSpriteCoords pos)
    {
        position = pos;
    }

    /// <summary>
    /// 로컬 관계를 고려해서 계산한 월드 좌표
    /// </summary>
    public CharSpriteCoords GetWorldPosition()
    {
        if (gameObject.ParentObject == null)
        {
            return position;
        }
        else
        {
            return gameObject.ParentObject.Transform.GetWorldPosition() + position;
        }
    }
}