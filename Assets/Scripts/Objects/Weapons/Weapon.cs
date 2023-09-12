using UnityEngine;

public abstract class Weapon : IContainerItem
{
    public ProjectileSettings ProjectileSettings;
    public WeaponSettings WeaponSettings;
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
            ProjectileSettings.Prefab,
            Vector3.zero,
            Quaternion.identity,
            ProjectileSettings.Damage,
            ProjectileSettings.Speed,
            ProjectileSettings.Lifetime
        );
    }

    public virtual void OnInsert() { }
    public virtual void OnRemove() { }
    public virtual void OnUpdate() {}
}