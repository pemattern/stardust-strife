public abstract class Upgrade
{
    public abstract string Title { get; } 
    public abstract string Description { get; }
    protected Unit Unit { get; private set; }
    public Upgrade(Unit unit)
    {
        Unit = unit;
    }

    public virtual void OnAttach()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnRemove()
    {

    }
}