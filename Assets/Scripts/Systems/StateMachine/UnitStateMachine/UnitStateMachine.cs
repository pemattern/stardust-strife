using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitStateMachine : IndefiniteStateMachine
{
    [HideInInspector] public Rigidbody Rigidbody;
    public IUnitController UnitController;

    [SerializeField] private bool _isPlayer;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        UnitController = _isPlayer ? InputHandler.Instance : GetComponent<AIController>();

        Init(new List<State>
        {
            new MoveState(this),
            new BoostState(this, 5),
            new FireState(this, _laserPrefab),
            new AlternateFireState(this, _missilePrefab)
        }, 0);
    }
}
