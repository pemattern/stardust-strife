using UnityEngine;

public class BoostState : State
{
    public override bool EntryCondition => _unitController.Boost;
    public override bool ExitCondition => _cooldown.IsCompleted;

    private bool _isToggle = false;
    private Awaitable _cooldown;
    private float _cooldownInSeconds;
    private IUnitController _unitController;

    public BoostState(UnitStateMachine stateMachine, float cooldownInSeconds) : base(stateMachine)
    {
        _unitController = stateMachine.UnitController;
        _cooldownInSeconds = cooldownInSeconds;
    }

    public override void Enter()
    {
        base.Enter();
        _cooldown = Awaitable.WaitForSecondsAsync(_cooldownInSeconds);
        Camera.main.Shake(4, 7, 1.75f, Ease.Triangle010);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        ((UnitStateMachine)StateMachine).Rigidbody.AddRelativeForce(Vector3.forward * 2f, ForceMode.Impulse);
    }
}