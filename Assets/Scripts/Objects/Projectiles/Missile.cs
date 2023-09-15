using UnityEngine;

public class Missile : Projectile
{
    private Rigidbody _rigidbody;
    private Transform _enemy;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();

        if (EnemyManager.CurrentTarget is not null)
            _enemy = EnemyManager.CurrentTarget.transform;
    }

    protected override void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * Speed;

        if (_enemy is null) return;

        Vector3 heading = _enemy.GetComponent<Rigidbody>().position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(heading);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, 4.2f));
    }
}
