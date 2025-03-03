class Component
{
    protected GameObject gameObject = null!;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; }
    }

    public bool enabled = true;

    public virtual void Start()
    {
         
    }

    public void RunFixedUpdate()
    {
        if (!enabled) return;
        FixedUpdate();
    }

    protected virtual void FixedUpdate() { }

    public virtual void OnDestroy()
    {

    }

    public void Destroy()
    {
        if (gameObject != null)
        {
            gameObject.DestroyComponent(this);
        }
    }
}