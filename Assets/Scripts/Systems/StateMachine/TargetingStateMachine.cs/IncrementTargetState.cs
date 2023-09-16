public class IncrementTargetState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget != null;
    public override bool ExitCondition => _targetingCompletion >= 1f || EnemyManager.CurrentTarget == null;

    private float _targetingCompletion;
    public NoTargetState(TargetingStateMachine targetingStateMachine) : base(targetingStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _targetingCompletion = 0f;
    }

    public override void Update()
    {
        base.Update();
        _targetingCompletion += ((TargetingStateMachine)_stateMachine).Speed * _targetingCompletion.deltaTime;

        if (_targetingCompletion >= 1f)
            EnemyManager.SetTargetEnemy()
    }
}