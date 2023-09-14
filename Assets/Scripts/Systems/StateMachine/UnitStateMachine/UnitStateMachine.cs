using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(UpgradeContainer), typeof(WeaponContainer))]
public class UnitStateMachine : IndefiniteStateMachine
{
    [HideInInspector] public Rigidbody Rigidbody { get; private set; }
    public IUnitController UnitController { get; private set; }
    public UpgradeContainer UpgradeContainer { get; private set; }
    public WeaponContainer WeaponContainer { get; private set; }

    [SerializeField] private bool _isPlayer;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        UnitController = _isPlayer ? InputHandler.Instance : GetComponent<AIController>();
        UpgradeContainer = GetComponent<UpgradeContainer>();
        WeaponContainer = GetComponent<WeaponContainer>();

        Init(new List<State>
        {
            new MoveState(this),
            new BoostState(this, 5),
            new FireState(this, WeaponContainer.Primary, true),
            new FireState(this, WeaponContainer.Alternate, false)
        }, 0);
    }
}
