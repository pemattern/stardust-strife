using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Shield))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] private GameObject _vfxDestroyed;

    [HideInInspector] public Health Health;
    [HideInInspector] public Shield Shield;
    public event Action<Unit> Destroyed;
    public int XPOnKill { get; private set; } = 20;

    protected virtual void OnEnable()
    {
        Health = GetComponent<Health>();
        Shield = GetComponent<Shield>();
        Health.ReachedZero += UnitDestroyed;
    }

    public virtual void TakeDamage(float amount)
    {
        if (Shield.Current == 0f) Health.Add(-amount);
        else Shield.Add(-amount);
    }

    private void UnitDestroyed()
    {
        Destroyed?.Invoke(this);
        Instantiate(_vfxDestroyed, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        Health.ReachedZero -= UnitDestroyed;
    }
}