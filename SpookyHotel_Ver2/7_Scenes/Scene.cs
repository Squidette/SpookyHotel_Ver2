class Scene
{
    protected List<GameObject> gameObjects;

    public Scene()
    {
        gameObjects = new List<GameObject>();
    }

    /// <summary>
    /// 씬에 게임오브젝트 추가
    /// </summary>
    public void AddGameObject(GameObject go)
    {
        if (!gameObjects.Contains(go))
        {
            gameObjects.Add(go);
        }
    }

    // debug
    public string ShowGameObjects()
    {
        string show = string.Empty;

        foreach (GameObject go in gameObjects)
        {
            show += go.Name;
            show += " ";
        }

        show += $"(count: {gameObjects.Count})";

        return show;
    }

    /// <summary>
    /// 씬에서 게임오브젝트 제거
    /// </summary>
    public bool RemoveGameObject(GameObject go)
    {
        if (go != null && gameObjects.Remove(go))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 이름이 일치하는 첫 번째 게임오브젝트를 반환
    /// </summary>
    public virtual GameObject? Find(string name)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.Name == name)
            {
                return gameObject;
            }
        }

        return null;
    }

    /// <summary>이 씬이 로드될때마다 한 번 실행</summary>
    public virtual void Start()
    {

    }

    /// <summary>매 FixedUpdate에서 지속적으로 실행</summary>
    public virtual void FixedUpdate()
    {
        //Debug.Log(GetType().ToString() + " running fixedupdate on " + gameObjects.Count + " objects");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.FixedUpdate();
        }
    }

    /// <summary>이 씬을 나갈때마다 한 번 실행</summary>
    public virtual void Exit()
    {

    }
}