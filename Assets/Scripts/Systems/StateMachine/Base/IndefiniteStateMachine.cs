public abstract class IndefiniteStateMachine : StateMachine
{
    void Update()
    {
        foreach (State state in _states)
        {
            if (state.EntryCondition && !state.IsActive)
                EnterState(state);

            if (!state.IsActive) continue;

            if (state.ExitCondition && state.IsActive)
                ExitState(state);

            state.Update();
        }
    }

    void FixedUpdate()
    {
        foreach (State state in _states)
        {
            if (!state.IsActive) continue;

            state.FixedUpdate();
        }
    }
}