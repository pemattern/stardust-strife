using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(Unit))]
public class UpgradeContainer : MonoBehaviour
{
    private Unit _unit;
    private List<Upgrade> _upgrades;

    void Start()
    {
        _unit = GetComponent<Unit>();
        _upgrades = new List<Upgrade>();
        AttachUpgrade<ShieldStabilizers>();
    }

    void Update()
    {
        foreach (Upgrade upgrade in _upgrades)
        {
            upgrade.OnUpdate();
        }
    }

    public void AttachUpgrade<T>() where T : Upgrade
    {
        T upgrade = (T)Activator.CreateInstance(typeof(T), new object[] { _unit });
        upgrade.OnAttach();
        _upgrades.Add(upgrade);
    }

    public void RemoveUpgrade<T>() where T : Upgrade
    {
        Upgrade upgrade = GetUpgrade<T>();
        upgrade.OnRemove();
        if (upgrade is not null) _upgrades.Remove(upgrade);
    }
    private Upgrade GetUpgrade<T>() where T : Upgrade
    {
        return _upgrades.Where(x => typeof(T) == x.GetType()).FirstOrDefault();
    }
}