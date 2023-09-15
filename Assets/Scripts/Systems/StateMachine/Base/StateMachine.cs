using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected List<State> _states;
    public event Action<State> Entered;
    public event Action<State> Exited;

    protected virtual void Init(List<State> states, int entryStateIndex)
    {
        _states = new List<State>();
        foreach (State state in states)
        {
            _states.Add(state);
        }
        _states[entryStateIndex].Enter();
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

    protected virtual void AddState(State state)
    {
        _states.Add(state);
    }
}