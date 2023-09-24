using UnityEngine;

[RequireComponent(typeof(Unit), typeof(UpgradeContainer))]
public class Weapon : MonoBehaviour
{
    public bool Locked;
    public bool IsPrimary => _isPrimary;
    public float Cooldown => _weaponSettings.Cooldown;
    public bool RequiresLockOn => _weaponSettings.RequiresLockOn;
    protected Unit _unit;
    [SerializeField] protected WeaponSettings _weaponSettings;
    [SerializeField] protected bool _isPrimary;
    protected UpgradeContainer _upgrades;

    protected virtual void Start()
    {
        _unit = GetComponent<Unit>();
        _upgrades = GetComponent<UpgradeContainer>();
        _upgrades.Refresh();

        if (RequiresLockOn) Locked = true;
        else Locked = false;
    }

    public virtual void Fire(Unit target = null)
    {
        Projectile.Fire
        (
            _unit,
            target,
            _weaponSettings.ProjectileSettings.Prefab,
            transform.position + transform.forward * 5,
            transform.rotation,
            _weaponSettings.ProjectileSettings.Damage,
            _weaponSettings.ProjectileSettings.Speed,
            _weaponSettings.ProjectileSettings.Lifetime
        );
    }

    public static bool TryGet<T>(Unit unit, out T result) where T : Weapon
    {
        Weapon[] weapons = unit.GetComponents<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            if (weapon is T)
            {
                result = (T)weapon;
                return true;
            }
        }
        result = null;
        return false;
    }
}