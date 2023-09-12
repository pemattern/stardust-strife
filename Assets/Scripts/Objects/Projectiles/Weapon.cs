using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ProjectileSettings ProjectileSettings;
    private Unit _unit;

    void Start() 
    {
        _unit = GetComponent<Unit>();
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
}