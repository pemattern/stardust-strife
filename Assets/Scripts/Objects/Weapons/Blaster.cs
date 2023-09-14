using UnityEngine;

public class Blaster : Weapon
{
    public override GameObject ProjectilePrefab { get; protected set; }
    public override GameObject WeaponPrefab { get; protected set; }
    public override float ProjectileSpeed { get; protected set; }
    public override float ProjectileDamage  { get; protected set; }
    public override float ProjectileLifetime  { get; protected set; }
    public override float WeaponCooldown  { get; protected set; }

    public Blaster (Unit unit) : base (unit) { }
}