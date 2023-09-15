using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Weapon : MonoBehaviour
{
    public bool IsPrimary => _isPrimary;
    public float Cooldown => _weaponSettings.Cooldown;
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
            transform.localRotation,
            _weaponSettings.ProjectileSettings.Damage,
            _weaponSettings.ProjectileSettings.Speed,
            _weaponSettings.ProjectileSettings.Lifetime
        );
    }
}