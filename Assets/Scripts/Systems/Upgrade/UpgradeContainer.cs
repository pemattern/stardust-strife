using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UpgradeContainer : Container<Upgrade>
{
    void Start()
    {
        Insert<ShieldStabilizers>();
    }
}