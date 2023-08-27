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
        if (StateMachine.gameObject.name == "Spaceship")
            Crosshair.UpdateCrosshair(StateMachine.gameObject.transform.position, StateMachine.gameObject.transform.forward, 500f, 0.5f);
    
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, 50f);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _rigidbody.AddRelativeForce(_unitController.Movement * 100f, ForceMode.Acceleration);
        _rigidbody.AddRelativeTorque(_unitController.Rotation * 2f, ForceMode.Acceleration);
    }
}