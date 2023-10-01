using System;

public class IonCharger : Weapon
{
    public Func<bool> DetonateCondition;
    protected override void Start()
    {
        base.Start();
        DetonateCondition = DetonateOnRetrigger;
    }
    public override void Fire(Unit target = null)
    {
        IonBomb ionBomb = (IonBomb) Projectile.Fire
        (
            _unit,
            target,
            _weaponSettings.ProjectileSettings.Prefab,
            transform.position + transform.forward * 2 + transform.up * -3,
            transform.rotation,
            _weaponSettings.ProjectileSettings.Damage,
            _weaponSettings.ProjectileSettings.Speed,
            _weaponSettings.ProjectileSettings.Lifetime            
        );
        ionBomb.DetonateCondition = DetonateCondition;
    }

    public bool DetonateOnRetrigger()
    {
        if (_isPrimary)
        {
            return InputHandler.Instance.Fire;
        }
        else
        {
            return InputHandler.Instance.AlternateFire;
        }
    }
}