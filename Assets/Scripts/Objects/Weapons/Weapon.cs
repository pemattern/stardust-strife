using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Weapon : MonoBehaviour
{
    public bool IsPrimary => _isPrimary;
    public float Cooldown => _weaponSettings.Cooldown;
    public bool RequiresLockOn => _weaponSettings.RequiresLockOn;
    private Unit _unit;
    [SerializeField] private WeaponSettings _weaponSettings;
    [SerializeField] private bool _isPrimary;

    void Start()
    {
        _unit = GetComponent<Unit>();
    }

    public void Fire()
    {
        Projectile.Fire
        (
            _unit,
            _weaponSettings.ProjectileSettings.Prefab,
            transform.position,
            transform.rotation,
            _weaponSettings.ProjectileSettings.Damage,
            _weaponSettings.ProjectileSettings.Speed,
            _weaponSettings.ProjectileSettings.Lifetime
        );
    }

    public virtual bool Locked()
    {
        return false;
    }
}