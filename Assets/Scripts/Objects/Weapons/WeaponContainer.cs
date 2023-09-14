using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    public Weapon Primary => _primaryWeapon;
    public Weapon Alternate => _alternateWeapon;
    private Weapon _primaryWeapon;
    private Weapon _alternateWeapon;
    private Unit _unit;
    void Start()
    {
        _unit = GetComponent<Unit>();
        SetPrimary(new Blaster(_unit));
        SetAlternate(new RocketLauncher(_unit));
    }

    void SetPrimary(Weapon weapon)
    {
        _primaryWeapon = weapon;
    }

    void SetAlternate(Weapon weapon)
    {
        _alternateWeapon = weapon;
    }
}