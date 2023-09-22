using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected float LifetimeInSeconds { get; private set; }
    protected float Speed { get; private set; }
    protected float Damage { get; private set; }
    protected Awaitable Lifetime { get; private set; }
    protected Unit ShotBy { get; private set; }
    protected Unit Target {get; private set;}

    [SerializeField] protected GameObject _vfxDamaged;

    protected virtual void Start()
    {
        Lifetime = Awaitable.WaitForSecondsAsync(LifetimeInSeconds);
    }

    protected virtual void Update()
    {
        if (Lifetime.IsCompleted) Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Unit unit))
        {
            if (ShotBy is EnemyUnit && unit is EnemyUnit) return;
            if (ShotBy is PlayerUnit && unit is PlayerUnit) return;

            Vector3 collisionPosition = collider.ClosestPointOnBounds(transform.position);
            Instantiate(_vfxDamaged, collisionPosition, Quaternion.identity);
            unit.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    public static void Fire(Unit shotBy, Unit target, GameObject prefab, Vector3 position, Quaternion rotation, float damage, float speed, float lifetime)
    {
        Projectile projectile = Instantiate(prefab, position, rotation).GetComponent<Projectile>();
        projectile.Damage = damage;
        projectile.ShotBy = shotBy;
        projectile.Speed = speed;
        projectile.LifetimeInSeconds = lifetime;
        projectile.Target = target;
        if(shotBy is PlayerUnit) Debug.Log("Fire");
    }
}
