using UnityEngine;

public abstract class Weapon
{
    public abstract GameObject ProjectilePrefab { get; protected set; }
    public abstract GameObject WeaponPrefab { get; protected set; }
    public abstract float ProjectileSpeed { get; protected set; }
    public abstract float ProjectileDamage { get; protected set; }
    public abstract float ProjectileLifetime { get; protected set; }
    public abstract float WeaponCooldown { get; protected set; }
    private Unit _unit;

    public Weapon (Unit unit)
    {
        _unit = unit;
    }

    public void Fire()
    {
        Projectile.Fire
        (
            _unit,
            ProjectilePrefab,
            Vector3.zero,
            Quaternion.identity,
            ProjectileDamage,
            ProjectileSpeed,
            ProjectileLifetime
        );
    }

    public virtual void OnInsert() { }
    public virtual void OnRemove() { }
    public virtual void OnUpdate() { }
}