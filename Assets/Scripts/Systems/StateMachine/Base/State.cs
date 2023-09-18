using System;
using UnityEngine;

public abstract class State
{
    public abstract bool EntryCondition { get; }
    public abstract bool ExitCondition { get; }
    public StateMachine StateMachine { get; private set; }
    
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
        if (StateMachine is AIStateMachine) Debug.Log("Entered: " + GetType());
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
        if (StateMachine is AIStateMachine) Debug.Log("Exited: " + GetType());
        IsActive = false;
        Exited?.Invoke();
    }
}