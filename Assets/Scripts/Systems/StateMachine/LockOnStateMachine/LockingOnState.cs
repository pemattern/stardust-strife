using UnityEngine;
public class LockingOnState : State
{
    public override bool EntryCondition => _lockOnStateMachine.LockingComplete == false && _lockOnStateMachine.Target != null;
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
    }

    public override void Update()
    {
        base.Update();
        _lockOnCompletion += _lockOnStateMachine.Speed * Time.deltaTime;

        _lockOnStateMachine.UpdateMarker(_lockOnCompletion);

        if (_lockOnCompletion >= 1f)
            _lockOnStateMachine.LockingComplete = true;

        if (_lockOnStateMachine.OutOfViewport(Camera.main.WorldToViewportPoint(_lockOnStateMachine.Target.transform.position)))
            _lockOnStateMachine.Reset(_lockOnStateMachine.Target);
    }
}