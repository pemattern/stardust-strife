using UnityEngine;
public class NoTargetState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget == null &&
        _lockOnStateMachine.Target == null;
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
    }
    
}