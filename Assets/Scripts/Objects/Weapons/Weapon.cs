using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Weapon : MonoBehaviour
{
    public bool Locked;
    public bool IsPrimary => _isPrimary;
    public float Cooldown => _weaponSettings.Cooldown;
    public bool RequiresLockOn => _weaponSettings.RequiresLockOn;
    private Unit _unit;
    [SerializeField] private WeaponSettings _weaponSettings;
    [SerializeField] private bool _isPrimary;

    void Start()
    {
        _unit = GetComponent<Unit>();

        if (RequiresLockOn) Locked = true;
        else Locked = false;
    }

    public void Fire(Unit target = null)
    {
        Projectile.Fire
        (
            _unit,
            target,
            _weaponSettings.ProjectileSettings.Prefab,
            transform.position,
            transform.rotation,
            _weaponSettings.ProjectileSettings.Damage,
            _weaponSettings.ProjectileSettings.Speed,
            _weaponSettings.ProjectileSettings.Lifetime
        );
    }
}