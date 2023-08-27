public class AdaptableAccelerators : Upgrade
{
    public override string Title { get; } = "Adaptable Accelerators";
    public override string Description { get; } =
        "Your boost can be toggled by holding/releasing the boost button.";

    public AdaptableAccelerators(Unit unit) : base(unit)
    {
        
    }
}