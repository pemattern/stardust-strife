using UnityEngine;

public class AIRepositionState : State
{
    private AIController _aiController;
    private Transform _transform;
    private Transform _playerTransform;
    private Vector3 _target;

    public AIRepositionState(StateMachine stateMachine, AIController aiController, Transform playerTransform) : base(stateMachine)
    {
        _aiController = aiController;
        _transform = StateMachine.transform;
        _playerTransform = playerTransform;
    }

    public override bool EntryCondition => Vector3.Distance(_transform.position, _playerTransform.position) < 50;
    public override bool ExitCondition => Vector3.Distance(_transform.position, _target) < 75;

    public override void Enter()
    {
        base.Enter();
        _aiController.GetRotation += TargetFarPosition;
        _aiController.GetMovement += Thrust;
        _aiController.GetFire += DontFire;

        _target = _playerTransform.position + Random.insideUnitSphere * 400;
    }

    public Vector3 TargetFarPosition(Vector3 prediction)
    {
        Vector3 target = (_target - StateMachine.transform.position).normalized;

        float pitch = -Vector3.Dot(target, _transform.up);
        float yaw = Vector3.Dot(target, _transform.right);

        float rollAdjustment = Vector3.Dot(target, _transform.forward);
        float roll = (Vector3.Dot(target, _transform.forward) < 0.9f) ? Mathf.Sign(rollAdjustment) * 1f : 0f;
        return new Vector3(pitch, yaw, roll);
    }

    private bool DontFire() => false;

    public Vector3 Thrust(Vector3 prediction)
    {
        return Vector3.forward;
    }

    public override void Exit()
    {
        base.Exit();
        _aiController.GetRotation -= TargetFarPosition;
        _aiController.GetMovement -= Thrust;
        _aiController.GetFire -= DontFire;
    }
}