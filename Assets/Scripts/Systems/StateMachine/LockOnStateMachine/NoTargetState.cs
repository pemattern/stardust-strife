public class NoTargetState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget == null &&
        ((LockOnStateMachine)StateMachine).Target == null;
    public override bool ExitCondition => !EntryCondition;
    public NoTargetState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine) { }
}