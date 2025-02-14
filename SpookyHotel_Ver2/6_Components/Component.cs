class Component
{
    protected GameObject gameObject = null!;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; }
    }

    public virtual void Start()
    {
         
    }

    public virtual void FixedUpdate()
    {

    }

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