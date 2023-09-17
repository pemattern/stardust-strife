using UnityEngine;

public class FireState : State
{
    public override bool EntryCondition => (_isPrimary ? _unitController.Fire : _unitController.AlternateFire) &&
        _weapon.Locked == false;
    public override bool ExitCondition => _cooldown?.IsCompleted??false;
    private Awaitable _cooldown;
    private float _cooldownInSeconds;
    private IUnitController _unitController;
    private Weapon _weapon;
    private bool _isPrimary;
    private ITargetProvider _targetProvider;

    public FireState(UnitStateMachine stateMachine, Weapon weapon, bool isPrimary) : base(stateMachine)
    { 
        _unitController = stateMachine.UnitController;
        _weapon = weapon;
        _isPrimary = isPrimary;
        _cooldownInSeconds = weapon.Cooldown;
        _targetProvider = stateMachine.TargetProvider;
    }

    public override void Enter()
    {
        base.Enter();

        _cooldown = Awaitable.WaitForSecondsAsync(_cooldownInSeconds);
        _weapon.Fire(_targetProvider?.Target);
    }
}