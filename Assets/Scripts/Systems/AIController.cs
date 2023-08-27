using UnityEngine;
using System;

public class AIController : MonoBehaviour, IUnitController
{
    public Vector3 Rotation { get; private set; }
    public Vector3 Movement { get; private set; }
    public bool Fire { get; private set; }
    public bool AlternateFire { get; private set; }
    public bool Boost { get; private set; }

    private GameObject _player;
    private TargetPrediction _playerPrediction;

    public event Func<Vector3, Vector3> GetRotation;
    public event Func<Vector3, Vector3> GetMovement;
    public event Func<bool> GetFire;
    public event Func<bool> GetBoost;

    void Start()
    {
        _player = GameObject.Find("Spaceship");
        _playerPrediction = new TargetPrediction(gameObject, _player);

        Movement = new Vector3(0f, 0f, 1f);
        Fire = false;
        AlternateFire = false;
    }

    void Update()
    {
        Rotation = GetRotation(_playerPrediction.GetAimDirection());
        Fire = GetFire();
        //Boost = GetBoost();
    }
}