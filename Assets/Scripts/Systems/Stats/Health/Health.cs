using UnityEngine;

public class Health : Stat
{
    public override void Add(float amount)
    {
        base.Add(amount);
        Debug.Log(amount);
    }
}