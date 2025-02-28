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

    public void FixedUpdate()
    {
        if (!enabled) return;
        FixedUpdateActions();
    }

    protected virtual void FixedUpdateActions() { }

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