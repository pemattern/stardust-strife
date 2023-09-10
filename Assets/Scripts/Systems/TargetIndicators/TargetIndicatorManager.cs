using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject _targetIndicatorPrefab;
    [SerializeField] private Canvas _canvas;

    private Dictionary<EnemyUnit, TargetIndicator> _targetIndicators = new Dictionary<EnemyUnit,TargetIndicator>();
    
    private void Start()
    {
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            TargetIndicator ti = Instantiate(_targetIndicatorPrefab, transform).GetComponent<TargetIndicator>();
            ti.Initialize(enemy.transform, Camera.main, _canvas);
            _targetIndicators.Add(enemy, ti);
            enemy.Destroyed += RemoveTargetIndicator;
        }
    }

    private void RemoveTargetIndicator(Unit enemy)
    {
        if (_targetIndicators.ContainsKey((EnemyUnit)enemy)) Destroy(_targetIndicators[(EnemyUnit)enemy].gameObject);
        _targetIndicators.Remove((EnemyUnit)enemy);
        enemy.Destroyed -= RemoveTargetIndicator;
    }
}
