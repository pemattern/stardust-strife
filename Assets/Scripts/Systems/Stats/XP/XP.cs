using System;
using UnityEngine;

public class XP : MonoBehaviour
{
    public float Current { get; private set; }
    public int Level => XPToLevel(Current);
    public float Normalized => (Current - LevelToXP(Level)) / LevelToXP(Level + 1);
    public event Action Changed;
    public event Action LevelUp;
    
    [SerializeField] private float _xpGainSpeed = 10f;
    private float _toBeAddedXP = 0f;

    void Start()
    {
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            enemy.Destroyed += AddXPFromKill;
        } 
    }

    void Update()
    {
        if (_toBeAddedXP > 0f)
        {
            float amount = Time.deltaTime * _xpGainSpeed;

            if (XPToLevel(Current + amount) > Level) LevelUp?.Invoke();

            Current += amount;
            _toBeAddedXP -= amount;

            Changed?.Invoke();
        }
        else
        {
            _toBeAddedXP = 0f;
        }
    }

    private void AddXPFromKill(Unit enemy)
    {
        AddXP(enemy.XPOnKill);
        enemy.Destroyed -= AddXPFromKill;
    }

    private void AddXP(float amount)
    {   
        _toBeAddedXP += amount;
    }

    private float LevelToXP(int level) => (Mathf.Pow(1.5f, (level - 1)) - 1) * 100;
    private int XPToLevel(float xp) => 1 + (int)((Mathf.Log((xp / 100f) + 1) / (Mathf.Log(1.5f))));

    void OnDisable()
    {
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            enemy.Destroyed -= AddXPFromKill;
        } 
    }
}
