using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _velocityRotation = Vector3.zero;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _speed;
    [SerializeField, Range(0f, 1f)] private float smoothTime;
    [SerializeField] private float _speedRotation;

    void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetTransform.position, ref _velocity, smoothTime);
        transform.localRotation = SmoothDampQuaternion(transform.localRotation, _targetTransform.rotation, ref _velocityRotation, smoothTime);
    }

    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler
        (
            Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }
}
