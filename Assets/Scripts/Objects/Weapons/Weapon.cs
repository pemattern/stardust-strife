using UnityEngine;

public abstract class Weapon : IContainerItem
{
    [SerializeField] private ProjectileSettings _projectileSettings;
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
            _projectileSettings.Prefab,
            Vector3.zero,
            Quaternion.identity,
            _projectileSettings.Damage,
            _projectileSettings.Speed,
            _projectileSettings.Lifetime
        );
    }

    public virtual void OnInsert() { }
    public virtual void OnRemove() { }
    public virtual void OnUpdate() { }
}