class CharSpriteRenderer : Component
{
    string charSpriteKey = string.Empty;
    public string CharSpriteKey
    {
        set { charSpriteKey = value; }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RenderableScene? rs = SceneManager.Instance.CurrentScene as RenderableScene;

        if (rs != null)
        {
            // 이 오브젝트의 월드 좌표
            CharSpriteCoords worldPosition = gameObject.Transform.GetWorldPosition();

            // 현재 씬의 카메라 좌표 가져오기
            CharSpriteCoords cameraPosition = rs.Camera.Transform.position;

            // 렌더
            ConsoleRenderer.Instance.Draw(charSpriteKey, worldPosition - cameraPosition);

            //Debug.Log("Draw " + gameObject.Name);
        }
    }
}