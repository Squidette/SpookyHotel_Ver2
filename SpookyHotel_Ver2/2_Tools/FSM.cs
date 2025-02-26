/// <summary>
/// 일반적인 FSM 클래스
/// </summary>
class FSM
{
    FSM_State? currentState;

    Dictionary<Type, FSM_State> loadedStates;

    public FSM()
    {
        loadedStates = new Dictionary<Type, FSM_State>();
        loadedStates.Clear();
    }

    public void ChangeState<T>()
        where T : FSM_State, new()
    {
        currentState?.Exit();

        if (loadedStates.ContainsKey(typeof(T)))
        {
            currentState = loadedStates[typeof(T)];
        }
        else
        {
            currentState = new T();
            currentState.StateMachine = this;
            loadedStates.Add(typeof(T), currentState);
        }

        currentState?.Enter();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}

class FSM_State
{
    protected FSM stateMachine = null!;
    public FSM StateMachine
    {
        set { stateMachine = value; }
    }

    public virtual void Enter()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}