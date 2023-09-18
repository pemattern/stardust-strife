using System.Collections.Generic;
using UnityEngine;

public class EnemyUIController : MonoBehaviour
{
    [SerializeField] private GameObject _enemyStatusBarPrefab;
    private Dictionary<EnemyUnit, EnemyStatusBar> _enemyBars;

    void Start()
    {
        _enemyBars = new Dictionary<EnemyUnit, EnemyStatusBar>(); 

        foreach(EnemyUnit enemy in EnemyManager.Enemies)
        {
            EnemyStatusBar bar = Instantiate(_enemyStatusBarPrefab, transform).GetComponent<EnemyStatusBar>();
            bar.Init(enemy);
            _enemyBars.Add(enemy, bar);
        }
    }
}