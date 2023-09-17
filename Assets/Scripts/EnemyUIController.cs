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
    void Update()
    {
        foreach(EnemyUnit enemy in EnemyManager.Enemies)
        {
            bool onScreen = !OutOfViewport(Camera.main.WorldToViewportPoint(enemy.transform.position));
            _enemyBars[enemy].gameObject.SetActive(onScreen);
        }
    }

    public bool OutOfViewport(Vector3 viewportPos)
    {
        return viewportPos.x < 0f ||
            viewportPos.x > 1f ||
            viewportPos.y < 0f ||
            viewportPos.y > 1f ||
            viewportPos.z < 0f;
    }
}