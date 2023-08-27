using UnityEngine;

public class TargetPrediction
{
    private Transform _sourceTransform;

    private Rigidbody _targetRigidbody;
    private Transform _targetTransform;

    private bool _predictedMovement;

    public TargetPrediction(GameObject source, GameObject target, bool predictedMovement = true)
    {
        _sourceTransform = source.transform;

        if (!target.TryGetComponent<Rigidbody>(out _targetRigidbody))
            throw new MissingComponentException("No Rigidbody found.");
        _targetTransform = target.transform;
        _predictedMovement = predictedMovement;
    }

    public Vector3 GetPredictedPosition() => _targetTransform.position + (_predictedMovement ? _targetRigidbody.velocity : Vector3.zero);

    public Vector3 GetAimDirection() => (GetPredictedPosition() - _sourceTransform.position).normalized;
}