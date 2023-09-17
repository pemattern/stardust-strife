using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _startingEnemies;
    [SerializeField] private LockOnStateMachine _lockOnStateMachine;
    public static List<EnemyUnit> Enemies { get; private set; } = new List<EnemyUnit>();
    public static EnemyUnit CurrentTarget { get; private set;}

    public EnemyUnit this[int index] => Enemies[index];

    void Start()
    {
        for (int i = 0; i < _startingEnemies; i++)
        {
            EnemyUnit enemy = Instantiate(_enemyPrefab, UnityEngine.Random.onUnitSphere * 50f, UnityEngine.Random.rotation, transform)
                .GetComponent<EnemyUnit>();
    	    enemy.Destroyed += Remove;
            Enemies.Add(enemy);
        }
    }

    void OnDisable()
    {
        foreach(EnemyUnit enemy in Enemies)
        {
            enemy.Destroyed -= Remove;
        }
    }

    void Remove(Unit enemy)
    {
        Enemies.Remove((EnemyUnit)enemy);
        Destroy(enemy.gameObject);
    }
}