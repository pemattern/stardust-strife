public class NoTargetState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget == null;
    public override bool ExitCondition => EnemyManager.CurrentTarget != null;
    public NoTargetState(TargetingStateMachine targetingStateMachine) : base(targetingStateMachine) { }
}