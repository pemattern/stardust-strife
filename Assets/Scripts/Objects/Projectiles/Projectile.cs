using System.Threading.Tasks;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected int LifetimeInMilliseconds { get; private set; }
    protected float Speed { get; private set; }
    protected int Damage { get; private set; }
    protected Task Lifetime { get; private set; }
    protected Unit ShotBy { get; private set; }

    [SerializeField] protected GameObject _vfxDamaged;

    protected virtual void Start()
    {
        Lifetime = Task.Delay(LifetimeInMilliseconds);
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
        if (collider.gameObject.TryGetComponent<Unit>(out Unit unit))
        {
            if (ShotBy is EnemyUnit && unit is EnemyUnit) return;
            if (ShotBy is PlayerUnit && unit is PlayerUnit) return;

            Vector3 collisionPosition = collider.ClosestPointOnBounds(transform.position);
            Instantiate(_vfxDamaged, collisionPosition, Quaternion.identity);
            unit.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    public static void Fire(Unit shotBy, GameObject prefab, Vector3 position, Quaternion rotation, int damage, float speed, float lifetime)
    {
        Projectile projectile = Instantiate(prefab, position, rotation).GetComponent<Projectile>();
        projectile.Damage = damage;
        projectile.ShotBy = shotBy;
        projectile.Speed = speed;
        projectile.LifetimeInMilliseconds = (int)(lifetime * 1000);
    }
}
