using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AIController))]
public class AIStateMachine : FiniteStateMachine
{
    private AIController _aiController;
    private Transform _playerTransform;

    void Start()
    {
        _aiController = GetComponent<AIController>();
        _playerTransform = GameObject.Find("Spaceship").transform;

        Init(new List<State>()
            {
                new AITargetPlayerState(this, _aiController, _playerTransform),
                new AIRepositionState(this, _aiController, _playerTransform)
            }, Random.Range(0, 2));
    }
}