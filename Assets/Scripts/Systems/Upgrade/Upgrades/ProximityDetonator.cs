public class ProximityDetonator : Upgrade
{
    public override string Title { get; } = "Proximity Detonator";
    public override string Description { get; } =
        "Ion bombs now automatically detonate when an enemy is nearby.";

    private Unit _unit;
    private IonCharger _ionCharger;

    public ProximityDetonator(Unit unit) : base(unit)
    {
        _unit = unit;
        Weapon.TryGet(unit, out _ionCharger);
    }

    public override void OnInsert()
    {
        base.OnInsert();
        if (_ionCharger != null)
        {
            _ionCharger.DetonateCondition = DetonateOnProximity;
        }
    }

    public override void OnRefresh()
    {
        base.OnRefresh();
        Weapon.TryGet(_unit, out _ionCharger);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (_ionCharger != null)
        {
            _ionCharger.DetonateCondition = _ionCharger.DetonateOnRetrigger;
        }
    }

    private bool DetonateOnProximity()
    {
        return false;
    }
}