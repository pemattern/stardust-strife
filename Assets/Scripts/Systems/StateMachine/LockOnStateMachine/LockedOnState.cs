using UnityEngine;
public class LockedOnState : State
{
    public override bool EntryCondition => _lockOnStateMachine.LockingComplete == true &&
        _lockOnStateMachine.Target != null;
    public override bool ExitCondition => !EntryCondition;
    private LockOnStateMachine _lockOnStateMachine;
    public LockedOnState(LockOnStateMachine lockOnStateMachine) : base(lockOnStateMachine)
    {
        _lockOnStateMachine = lockOnStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _lockOnStateMachine.SetWeaponLockState(false);
    }

    public override void Update()
    {
        base.Update();
        _lockOnStateMachine.UpdateMarker(1f);

        if (_lockOnStateMachine.OutOfViewport(Camera.main.WorldToViewportPoint(_lockOnStateMachine.Target.transform.position)))
            _lockOnStateMachine.Reset(_lockOnStateMachine.Target);
    }
}