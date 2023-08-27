using System;

public abstract class State
{
    public abstract bool EntryCondition { get; }
    public abstract bool ExitCondition { get; }

    protected StateMachine StateMachine { get; private set; }
    
    public bool IsActive { get; private set; }
    public event Action Entered;
    public event Action Updated;
    public event Action FixedUpdated;
    public event Action Exited;

    public State(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
        IsActive = false;
    }

    public virtual void Enter()
    {
        //Debug.Log($"Entered State: {this.ToString()}");
        IsActive = true;
        Entered?.Invoke();
    }

    public virtual void Update()
    {
        Updated?.Invoke();
    }
    public virtual void FixedUpdate()
    {
        FixedUpdated?.Invoke();
    }

    public virtual void Exit()
    {
        //Debug.Log($"Exited State: {this.ToString()}");
        IsActive = false;
        Exited?.Invoke();
    }
}