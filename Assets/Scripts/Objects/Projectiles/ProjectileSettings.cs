using UnityEngine;

[CreateAssetMenu()]
public class ProjectileSettings : ScriptableObject
{
    public GameObject Prefab;
    public float Speed;
    public float Lifetime;
    public int Damage;
}