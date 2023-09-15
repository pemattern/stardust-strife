using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(UpgradeContainer), typeof(Weapon))]
public class UnitStateMachine : IndefiniteStateMachine
{
    [HideInInspector] public Rigidbody Rigidbody { get; private set; }
    public IUnitController UnitController { get; private set; }
    public UpgradeContainer UpgradeContainer { get; private set; }
    private Weapon _primaryWeapon;
    private Weapon _alternateWeapon;
    [SerializeField] private bool _isPlayer;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        UnitController = _isPlayer ? InputHandler.Instance : GetComponent<AIController>();
        UpgradeContainer = GetComponent<UpgradeContainer>();
        
        Weapon[] weapons = GetComponents<Weapon>();
        if (weapons.Length > 2) throw new System.Exception("Too many weapons equipped.");
        foreach (Weapon weapon in weapons)
        {
            if (weapon.IsPrimary)
                _primaryWeapon = weapon;
            else
                _alternateWeapon = weapon;
        }

        Init(new List<State>
        {   
            new MoveState(this),
            new BoostState(this, 5),
            new FireState(this, _primaryWeapon, true)
        }, 0);

        if (_alternateWeapon != null)
            AddState(new FireState(this, _alternateWeapon, false));
    }
}
