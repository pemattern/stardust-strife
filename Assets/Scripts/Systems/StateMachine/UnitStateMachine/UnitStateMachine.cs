using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(UpgradeContainer))]
public class UnitStateMachine : IndefiniteStateMachine
{
    [HideInInspector] public Rigidbody Rigidbody { get; private set; }
    public IUnitController UnitController { get; private set; }
    public UpgradeContainer UpgradeContainer { get; private set; }

    [SerializeField] private bool _isPlayer;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        UnitController = _isPlayer ? InputHandler.Instance : GetComponent<AIController>();
        UpgradeContainer = GetComponent<UpgradeContainer>();

        Init(new List<State>
        {
            new MoveState(this),
            new BoostState(this, 5),
            new FireState(this, _laserPrefab),
            new AlternateFireState(this, _missilePrefab)
        }, 0);
    }
}
