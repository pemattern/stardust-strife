using UnityEngine;

public class AlternateFireState : State
{
    public override bool EntryCondition => _unitController.AlternateFire && EnemyManager.CurrentTarget is not null;
    public override bool ExitCondition => _cooldown?.IsCompleted??false;

    private Awaitable _cooldown;
    private float _cooldownInSeconds = 10f;

    private IUnitController _unitController;
    private GameObject _projectile;

    private Overheat _overheat;

    public AlternateFireState(UnitStateMachine stateMachine, GameObject projectile) : base(stateMachine)
    { 
        _unitController = stateMachine.UnitController;
        _projectile = projectile;
        //_overheat = new Overheat();
    }

    public override void Enter()
    {
        base.Enter();

        //if (_overheat.Locked) return;

        _cooldown = Awaitable.WaitForSecondsAsync(_cooldownInSeconds);

        Projectile.Fire
        (
            StateMachine.gameObject.GetComponent<Unit>(),
            _projectile,
            StateMachine.gameObject.transform.position, 
            StateMachine.gameObject.transform.rotation,
            5,
            150f,
            5f           
        );

        //_overheat.Increment();
    }
}