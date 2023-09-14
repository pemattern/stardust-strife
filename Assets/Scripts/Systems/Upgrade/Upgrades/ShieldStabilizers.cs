public class ShieldStabilizers : Upgrade
{
    public override string Title { get; } = "Shield Stabilizers";
    public override string Description { get; } =
        $"Increases maximum shield capacity by {_max}, reduces shield recharge delay by {_rechargeDelay}s and increases shield recharge speed by {_rechargeSpeed}/s.";

    private const float _max = 1, _rechargeDelay = -0.25f, _rechargeSpeed = 0.5f;
    private Shield _shield;
    public ShieldStabilizers(Unit unit) : base(unit)
    {
        _shield = unit.GetComponent<Shield>();
    }
    public override void OnInsert()
    { 
        _shield.AddMax(_max);
        _shield.AddRechargeDelay(_rechargeDelay);
        _shield.AddRechargeSpeed(_rechargeSpeed);
    }

    public override void OnRemove()
    {
        _shield.AddMax(-_max);
        _shield.AddRechargeDelay(-_rechargeDelay);
        _shield.AddRechargeSpeed(-_rechargeSpeed);
    }
}
