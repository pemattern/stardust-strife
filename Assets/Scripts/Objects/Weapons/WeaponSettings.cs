using UnityEngine;

[CreateAssetMenu()]
public class WeaponSettings : ScriptableObject
{
    public ProjectileSettings ProjectileSettings;
    public GameObject Prefab;
    public float Cooldown;
}