using UnityEditor;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public RocketLauncher (Unit unit) : base (unit) { }

    public override GameObject ProjectilePrefab { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override GameObject WeaponPrefab { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float ProjectileSpeed { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float ProjectileDamage { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float ProjectileLifetime { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float WeaponCooldown { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
}