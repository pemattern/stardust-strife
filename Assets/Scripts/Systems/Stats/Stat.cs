using System;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    public float Current { get; private set; }
    public float Max => _maxValue;

    public event Action Changed;
    public event Action Decreased;
    public event Action Increased;
    public event Action ReachedZero;
    public event Action<float> Overkill;

    public float Normalized => Current > 0f ? Current / Max : 0f;

    [SerializeField] private float _maxValue;

    public virtual void Start()
    {
        Current = _maxValue;
    }

    public virtual void AddMax(float amount)
    {
        _maxValue += amount;
        Current += amount;
    }

    public virtual void Add(float amount)
    {
        float before = Current;
        Current += amount;
        Current = Mathf.Clamp(Current, 0f, _maxValue);
        
        if (amount != 0f) Changed?.Invoke();

        float overkill = before + amount;
        if (overkill < 0f) Overkill?.Invoke(overkill);

        if (Current > before) Increased?.Invoke();
        if (Current < before) Decreased?.Invoke();
        if (Current == 0f) ReachedZero?.Invoke();
    }
}