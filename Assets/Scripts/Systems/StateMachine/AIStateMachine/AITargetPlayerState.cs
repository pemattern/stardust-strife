using System.Threading.Tasks;
using UnityEngine;

public class AITargetPlayerState : State
{
    private AIController _aiController;
    private Transform _playerTransform;
    private Transform _transform;
    private Task _task;

    public AITargetPlayerState(StateMachine stateMachine, AIController aiController, Transform playerTransform) : base(stateMachine)
    { 
        _transform = StateMachine.transform; 
        _playerTransform = playerTransform;
        _aiController = aiController;
    }

    public override bool EntryCondition => Vector3.Distance(_transform.position, _playerTransform.position) < 500;
    public override bool ExitCondition => _task.IsCompleted;

    public override void Enter()
    {
        base.Enter();
        _aiController.GetRotation += TargetPlayer;
        _aiController.GetMovement += Thrust;
        _aiController.GetFire += Fire;
        _task = Task.Delay(Random.Range(8000, 10000));
    }

    private Vector3 TargetPlayer(Vector3 prediction)
    {
        float pitch = -Vector3.Dot(prediction, _transform.up);
        float yaw = Vector3.Dot(prediction, _transform.right);

        float rollAdjustment = Vector3.Dot(prediction, _transform.forward);
        float roll = (Vector3.Dot(prediction, _transform.forward) < 0.9f) ? Mathf.Sign(rollAdjustment) * 1f : 0f;
        return new Vector3(pitch, yaw, roll);
    }

    private bool Fire() => Vector3.Dot(_transform.forward, (_playerTransform.position - _transform.position).normalized) > 0.98f;

    private Vector3 Thrust(Vector3 prediction)
    {
        return Vector3.forward;
    }

    public override void Exit()
    {
        base.Exit();
        _aiController.GetRotation -= TargetPlayer;
        _aiController.GetMovement -= Thrust;
        _aiController.GetFire -= Fire;
    }
}