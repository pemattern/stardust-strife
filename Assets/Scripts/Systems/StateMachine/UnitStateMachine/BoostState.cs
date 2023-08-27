using UnityEngine;
using System.Threading.Tasks;

public class BoostState : State
{
    public override bool EntryCondition => _unitController.Boost;
    public override bool ExitCondition => Operation.IsCompleted;

    public Task Operation { get; private set; }
    public int CooldownInMilliseconds { get; private set; }

    private IUnitController _unitController;

    public BoostState(UnitStateMachine stateMachine, int cooldownInSeconds) : base(stateMachine)
    {
        _unitController = stateMachine.UnitController;
        CooldownInMilliseconds = cooldownInSeconds * 1000;
    }

    public override void Enter()
    {
        base.Enter();
        Operation = Task.Delay(CooldownInMilliseconds);
        Camera.main.Shake(4, 7, 1.75f, Ease.Triangle010);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        ((UnitStateMachine)StateMachine).Rigidbody.AddRelativeForce(Vector3.forward * 2f, ForceMode.Impulse);
    }
}