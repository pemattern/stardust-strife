using System;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    public float Current { get; private set; }
    public float Max { get ; private set; }

    public event Action Changed;
    public event Action Decreased;
    public event Action Increased;
    public event Action ReachedZero;

    public float Normalized => Current > 0f ? Current / Max : 0f;

    [SerializeField] private float _maxValue;

    void Start()
    {
        Current = _maxValue;
        Max = _maxValue;
    }

    public virtual void AddMax(float amount)
    {
        Max += amount;
        Add(amount);
    }

    public virtual void Add(float amount)
    {
        float before = Current;
        Current += amount;
        Current = Mathf.Clamp(Current, 0f, Max);
        Changed?.Invoke();

        if (Current > before) Increased?.Invoke();
        if (Current < before) Decreased?.Invoke();
        if (Current == 0f) ReachedZero?.Invoke();
    }
}