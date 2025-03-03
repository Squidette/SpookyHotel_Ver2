/// <summary>
/// 유니티의 MonoBehaviour 같은 것
/// </summary>
class Behavior : Component
{
    public override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void Destroy(GameObject go)
    {
        SceneManager.Instance.CurrentScene.RemoveGameObject(go);
    }

    protected void Destroy(Component c)
    {
        if (c != null)
        {
            c.Destroy();
        }
    }
}