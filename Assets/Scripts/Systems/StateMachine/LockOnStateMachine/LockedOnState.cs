using UnityEngine;
public class LockedOnState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget != null;
    public override bool ExitCondition => EnemyManager.CurrentTarget == null ||
        OutOfViewport(Camera.main.WorldToViewportPoint(_lockOnStateMachine.Target.transform.position));

    private LockOnStateMachine _lockOnStateMachine;
    public LockedOnState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _lockOnStateMachine.Image.color = _lockOnStateMachine.LockedOnColor;
        _lockOnStateMachine.Image.fillAmount = 1f;
    }

    bool OutOfViewport(Vector3 viewportPos)
    {
        return viewportPos.x < 0f ||
            viewportPos.x > 1f ||
            viewportPos.y < 0f ||
            viewportPos.y > 1f ||
            viewportPos.z < _lockOnStateMachine.MinDistance;
    }
}