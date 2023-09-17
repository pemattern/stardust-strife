using UnityEngine;
public class NoTargetState : State
{
    public override bool EntryCondition => _lockOnStateMachine.Target == null;
    public override bool ExitCondition => !EntryCondition;
    private LockOnStateMachine _lockOnStateMachine;
    public NoTargetState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Update()
    {
        base.Update();
        _lockOnStateMachine.SetTarget();

        if (_lockOnStateMachine.Target != null)
            _lockOnStateMachine.Target.Destroyed += _lockOnStateMachine.Reset;
    }
    
}