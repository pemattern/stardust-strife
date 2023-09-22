using UnityEngine;

[RequireComponent(typeof(XP))]
public class PlayerUnit : Unit
{
    public XP XP { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        XP = GetComponent<XP>();
    }

    private void Update()
    {
        Crosshair.Refresh(transform.position, transform.forward, 500f, 0.5f);
    }
}