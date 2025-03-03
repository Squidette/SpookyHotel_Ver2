/// <summary>
/// 하나의 씬마다 하나의 인스턴스로 관리하는 씬매니저
/// </summary>
class SceneManager
{
    // 싱글턴
    static SceneManager instance = null!;
    public static SceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SceneManager();
            }
            return instance;
        }
    }

    // 현재 씬
    Scene currentScene = null!;
    public Scene CurrentScene { get { return currentScene; } }

    // 다음으로 로드할 씬
    Scene? nextScene = null;

    // 항상 함께 돌아가고있는, DontDestroyOnLoad씬
    Scene dontDestroyOnLoadScene;
    public Scene DontDestroyOnLoadScene
    {
        get { return dontDestroyOnLoadScene; }
    }

    SceneManager()
    {
        dontDestroyOnLoadScene = new Scene();

        // 로비로 시작
        LoadScene<Lobby>();
    }

    /// <summary>
    /// 씬 로드 
    /// </summary>
    /// <param name="move">씬을 바꿀 것인지(false인 경우 로드만 한다)</param>
    public void LoadScene<T>()
        where T : Scene, new()
    {
        // 이미 전환이 예약된 씬이 있다면 리턴
        if (nextScene != null) return;

        // 씬 바꾸기 예약 (흐름 제어를 위해 실제 바꾸는건 다음 FixeUpdate에서 모아서 한다)
        nextScene = new T();
    }

    public void FixedUpdate()
    {
        if (nextScene != null)
        {
            currentScene?.Exit();
            currentScene = nextScene;
            currentScene.Start();
            nextScene = null;
        }

        currentScene?.FixedUpdate();
        dontDestroyOnLoadScene.FixedUpdate();

        //// debug
        if (InputManager.Instance.GetKey(ConsoleKey.N))
        {
            Debug.Log(DontDestroyOnLoadScene.ShowGameObjects());
        }
        if (InputManager.Instance.GetKey(ConsoleKey.M))
        {
            Debug.Log(currentScene!.ShowGameObjects());
        }
    }

    public void DontDestroyOnLoad(GameObject go)
    {
        // CurrentScene의 오브젝트를 DontDestroyOnLoadScene으로 옮긴다
        if (currentScene != null)
        {
            if (currentScene.RemoveGameObject(go))
            {
                go.ParentObject = null;
                dontDestroyOnLoadScene.AddGameObject(go);
            }
            else
            {
                Debug.Log("SceneManager_DontdestroyOnLoad[failed to remove gameObject from the original scene]");
            }
        }
        else
        {
            Debug.Log("SceneManager_DontdestroyOnLoad[currentScene null]");
        }
    }
}