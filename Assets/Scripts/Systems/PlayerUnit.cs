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

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        //Camera.main.Shake(1f, 0.2f, 0.4f, Ease.CubeOut);
    }
}