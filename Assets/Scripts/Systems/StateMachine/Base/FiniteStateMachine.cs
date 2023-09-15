using System.Collections.Generic;

public abstract class FiniteStateMachine : StateMachine
{
    public State CurrentState { get; private set; }

    protected override void Init(List<State> states, int entryStateIndex)
    {
        base.Init(states, entryStateIndex);
        CurrentState = _states[entryStateIndex];
    }

    void Update()
    {
        foreach (State state in _states)
        {
            if (state.EntryCondition && CurrentState.ExitCondition && !state.IsActive)
                EnterState(state);
        }

        CurrentState.Update();
    }

    void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }

    protected override void EnterState(State state)
    {
        _states.ForEach(x => ExitState(x));
        CurrentState = state;
        base.EnterState(state);
    }
}