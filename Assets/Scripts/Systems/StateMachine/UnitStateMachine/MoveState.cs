using UnityEngine;

public class MoveState : State
{
    public override bool EntryCondition => true;
    public override bool ExitCondition => false;

    private IUnitController _unitController;
    private Rigidbody _rigidbody;

    public MoveState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        _unitController = stateMachine.UnitController;
        _rigidbody = stateMachine.Rigidbody;
    }

    public override void Update()
    {
        base.Update();

        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, 25f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _rigidbody.AddRelativeForce(_unitController.Movement * 50f, ForceMode.Acceleration);
        _rigidbody.AddRelativeTorque(_unitController.Rotation * 1f, ForceMode.Acceleration);
    }
}