using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(Unit))]
public class Container<T> : MonoBehaviour where T : IContainerItem
{
    private Unit _unit;
    private List<T> _items;

    public T this[int i] => _items[i];

    void Start()
    {
        _unit = GetComponent<Unit>();
        _items = new List<T>();
    }

    void Update()
    {
        foreach (T item in _items)
        {
            item.OnUpdate();
        }
    }

    public void Insert<TConcrete>() where TConcrete : T
    {
        TConcrete item = (TConcrete)Activator.CreateInstance(typeof(TConcrete), new object[] { _unit });
        item.OnInsert();
        _items.Add(item);
    }

    public void Remove<TConcrete>() where TConcrete : T
    {
        if (!TryGet<TConcrete>(out TConcrete item))
            return;
        item.OnRemove();
        if (item is not null) _items.Remove(item);
    }

    public int Count()
    {
        return _items.Count();
    }

    public IEnumerable<T> All()
    {
        return _items;
    }

    public bool Has<TConcrete>() where TConcrete : T
    {
        return _items.Where(x => typeof(TConcrete) == x.GetType()).Any();
    }

    public bool TryGet<TConcrete>(out TConcrete item) where TConcrete : T
    {
        item = (TConcrete) _items.Where(x => typeof(TConcrete) == x.GetType()).FirstOrDefault();
        
        if (item == null) return false;
        return true;
    }
}