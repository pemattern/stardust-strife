using UnityEngine;
public class LockedOnState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget != null;
    public override bool ExitCondition => EnemyManager.CurrentTarget == null;

    private LockOnStateMachine _lockOnStateMachine;
    public LockedOnState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Update()
    {
        base.Update();
        _lockOnStateMachine.UpdateMarker(1f);

        if (OutOfViewport(Camera.main.WorldToViewportPoint(EnemyManager.CurrentTarget.transform.position)))
        {
            EnemyManager.SetTargetEnemy(null);
            _lockOnStateMachine.RemoveTarget();
        }
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