using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public List<State> States { get; private set; } = new List<State>();
    public event Action<State> Entered;
    public event Action<State> Exited;

    protected virtual void Init(List<State> states, int entryStateIndex)
    {
        foreach (State state in states)
        {
            States.Add(state);
        }
        States[entryStateIndex].Enter();
    }

    protected virtual void EnterState(State state)
    {
        Entered?.Invoke(state);
        state.Enter();
    }

    protected virtual void ExitState(State state)
    {
        Exited?.Invoke(state);
        state.Exit();
    }
}