using System.Threading.Tasks;
using UnityEngine;

public class FireState : State
{
    public override bool EntryCondition => _unitController.Fire;
    public override bool ExitCondition => _fire?.IsCompleted??false;

    private Task _fire;
    private int _cooldownTime = 150;

    private IUnitController _unitController;
    private GameObject _laser;

    private Overheat _overheat;

    public FireState(UnitStateMachine stateMachine, GameObject laser) : base(stateMachine)
    { 
        _unitController = stateMachine.UnitController;
        _laser = laser;
        //_overheat = new Overheat();
    }

    public override void Enter()
    {
        base.Enter();

        //if (_overheat.Locked) return;

        _fire = Task.Delay(_cooldownTime);

        Projectile.Fire
        (
            StateMachine.gameObject.GetComponent<Unit>(),
            _laser,
            StateMachine.gameObject.transform.right * .5f + StateMachine.gameObject.transform.position, 
            StateMachine.gameObject.transform.rotation,
            1,
            750f,
            0.33f           
        );

        Projectile.Fire
        (
            StateMachine.gameObject.GetComponent<Unit>(),
            _laser,
            StateMachine.gameObject.transform.right * -.5f + StateMachine.gameObject.transform.position, 
            StateMachine.gameObject.transform.rotation,
            1,
            750f,
            0.33f           
        );

        //_overheat.Increment();
    }
}