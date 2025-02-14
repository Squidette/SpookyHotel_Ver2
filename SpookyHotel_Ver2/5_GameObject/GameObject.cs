class GameObject
{
    // 이름
    string name;
    public string Name { get { return name; } }

    // 부모 게임오브젝트 (있다면)
    GameObject? parentObject;
    public GameObject? ParentObject
    {
        get { return parentObject; }
        set { parentObject = value; }
    }

    // 트랜스폼
    Transform transform;
    public Transform Transform { get { return transform; } }

    // 가지고 있는 모든 컴포넌트들
    List<Component> components;

    public GameObject(string name, CharSpriteCoords initialPos = new CharSpriteCoords())
    {
        this.name = name;

        // 게임오브젝트 생성시 기본으로 트랜스폼 추가
        transform = new Transform(initialPos);
        transform.GameObject = this;
        components = [transform];
    }

    /// <summary>
    /// 컴포넌트 추가
    /// </summary>
    public void AddComponent<T>()
        where T : Component, new()
    {
        // 이 함수로 추가 불가능한 컴포넌트
        if (typeof(T) == typeof(Component)) return;
        if (typeof(T) == typeof(Transform)) return;

        // 이 함수로 추가는 가능하지만 다수 생성은 불가능한 컴포넌트
        if (typeof(T) == typeof(CharSpriteRenderer) && AlreadyHasComponent<CharSpriteRenderer>()) return;

        // 컴포넌트 추가
        T newComponent = new T();
        newComponent.GameObject = this;
        newComponent.Start();
        components.Add(newComponent);
    }

    /// <summary>
    /// 컴포넌트 가져오기
    /// </summary>
    public T GetComponent<T>()
        where T : Component
    {
        foreach (Component component in components)
        {
            if (component is T subtypeComponent)
            {
                return subtypeComponent;
            }
        }
        return null!;
    }

    /// <summary>
    /// 이 게임오브젝트의 컴포넌트를 제거 (검색해서 나오는 첫 번째 컴포넌트)
    /// </summary>
    public bool DestroyComponent(Component component)
    {
        if (components.Remove(component))
        {
            component.OnDestroy();
            return true;
        }
        else return false;
    }

    /// <summary>
    /// 이 게임오브젝트의 모든 컴포넌트를 제거
    /// </summary>
    public void DestroyAllComponents()
    {
        foreach (Component component in components)
        {
            if (component != null) component.OnDestroy();
        }
        components.Clear();
    }

    public void FixedUpdate()
    {
        foreach (Component component in components)
        {
            component.FixedUpdate();
        }
    }

    bool AlreadyHasComponent<T>()
    {
        return components.Any(component => component.GetType() == typeof(T));
    }

    /// <summary>
    /// 최상위 부모 가져오기
    /// </summary>
    public GameObject GetRootParent()
    {
        GameObject go = this;
        while (go.parentObject != null)
        {
            go = go.parentObject;
        }
        return go;
    }
}