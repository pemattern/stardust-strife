using UnityEngine;

public class FireState : State
{
    public override bool EntryCondition => _isPrimary ? _unitController.Fire : _unitController.AlternateFire;
    public override bool ExitCondition => _cooldown?.IsCompleted??false;

    private Awaitable _cooldown;
    private float _cooldownInSeconds;

    private IUnitController _unitController;
    private Weapon _weapon;
    private bool _isPrimary;

    public FireState(UnitStateMachine stateMachine, Weapon weapon, bool isPrimary) : base(stateMachine)
    { 
        _unitController = stateMachine.UnitController;
        _weapon = weapon;
        _isPrimary = isPrimary;
        _cooldownInSeconds = weapon.WeaponSettings.Cooldown;
    }

    public override void Enter()
    {
        base.Enter();

        _cooldown = Awaitable.WaitForSecondsAsync(_cooldownInSeconds);

        // Projectile.Fire
        // (
        //     StateMachine.gameObject.GetComponent<Unit>(),
        //     _laser ,
        //     StateMachine.gameObject.transform.right * .5f + StateMachine.gameObject.transform.position, 
        //     StateMachine.gameObject.transform.rotation,
        //     1,
        //     750f,
        //     0.33f           
        // );

        _weapon.Fire();
    }
}