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

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        //Camera.main.Shake(0.7f, 3, 3, Ease.CubeOut);
    }
}