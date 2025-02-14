class CharSpriteRenderer : Component
{
    string charSpriteKey = string.Empty;
    public string CharSpriteKey
    {
        set { charSpriteKey = value; }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // 이 오브젝트의 월드 좌표
        CharSpriteCoords worldPosition = gameObject.Transform.GetWorldPosition();

        // 현재 씬의 카메라 좌표 가져오기
        CharSpriteCoords cameraPosition = SceneManager.Instance.CurrentScene.Camera.Transform.position;

        // 렌더
        ConsoleRenderer.Instance.Draw(charSpriteKey, worldPosition - cameraPosition);

        //Debug.Log("Draw " + gameObject.Name);
    }
}