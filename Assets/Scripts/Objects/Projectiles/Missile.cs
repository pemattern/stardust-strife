using UnityEngine;

public class Missile : Projectile
{
    [SerializeField] private float _maxDegreesDelta;
    private Rigidbody _rigidbody;
    private Rigidbody _targetRigidbody;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();

        if (Target is not null)
            _targetRigidbody = Target.GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * Speed;

        if (Target is null) return;

        Vector3 heading = _targetRigidbody.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(heading);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _maxDegreesDelta)); //4.2f
    }
}
