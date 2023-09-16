using UnityEngine;
public class NoTargetState : State
{
    public override bool EntryCondition => EnemyManager.CurrentTarget == null &&
        ((LockOnStateMachine)StateMachine).Target == null;
    public override bool ExitCondition => !EntryCondition;
    private LockOnStateMachine _lockOnStateMachine;
    public NoTargetState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Update()
    {
        base.Update();
        _lockOnStateMachine.Target = GetTarget();
    }
    EnemyUnit GetTarget()
    {
        EnemyUnit enemyUnit = null;
        float winnerDistance = 1f;
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            if (WithinCenteredRadius(viewportPos))
            {
                float distance = Vector2.Distance(viewportPos, new Vector2(0.5f, 0.5f));

                if (distance < winnerDistance)
                {
                    winnerDistance = distance;
                    enemyUnit = enemy;
                }
            }
        }
        return enemyUnit;       
    }

    bool WithinCenteredRadius(Vector3 viewportPos)
    {
        return viewportPos.x > 0.5f - _lockOnStateMachine.Radius &&
            viewportPos.x < 0.5f + _lockOnStateMachine.Radius &&
            viewportPos.y > 0.5f - _lockOnStateMachine.Radius &&
            viewportPos.y < 0.5f + _lockOnStateMachine.Radius &&
            viewportPos.z > _lockOnStateMachine.MinDistance && viewportPos.z < _lockOnStateMachine.MaxDistance;
    }
}