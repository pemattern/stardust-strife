using UnityEngine;
public class LockingOnState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget == null && _lockOnStateMachine.Target != null;
    public override bool ExitCondition => _lockOnCompletion >= 1f || _lockOnStateMachine.Target == null;
    private LockOnStateMachine _lockOnStateMachine;
    private float _lockOnCompletion;
    public LockingOnState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _lockOnCompletion = 0f;
        _lockOnStateMachine.Image.color = _lockOnStateMachine.LockingOnColor;
    }

    public override void Update()
    {
        base.Update();
        _lockOnCompletion += _lockOnStateMachine.Speed * Time.deltaTime;
        _lockOnCompletion /= _lockOnStateMachine.Duration;

        _lockOnStateMachine.Image.fillAmount = _lockOnCompletion;

        if (_lockOnCompletion >= 1f)
            EnemyManager.SetTargetEnemy(_lockOnStateMachine.Target);
    }
}