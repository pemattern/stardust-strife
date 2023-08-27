using System.Collections.Generic;
using System.Linq;

public abstract class FiniteStateMachine : StateMachine
{
    public State CurrentState { get; private set; }

    protected override void Init(List<State> states, int entryStateIndex)
    {
        base.Init(states, entryStateIndex);
        CurrentState = States[entryStateIndex];
    }

    void Update()
    {
        foreach (State state in States)
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
        States.ForEach(x => ExitState(x));
        CurrentState = state;
        base.EnterState(state);
    }
}