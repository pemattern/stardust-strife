using UnityEngine;

public class WeaponContainer : Container<Weapon> 
{
    void Start()
    {
        Insert<Blaster>();
        Insert<RocketLauncher>();
    }
}