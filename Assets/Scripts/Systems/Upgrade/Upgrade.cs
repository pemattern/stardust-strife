public abstract class Upgrade : IContainerItem
{
    public abstract string Title { get; } 
    public abstract string Description { get; }
    protected Unit Unit { get; private set; }
    public Upgrade(Unit unit)
    {
        Unit = unit;
    }

    public virtual void OnInsert()
    {

    }

    public virtual void OnRefresh()
    {
        
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnRemove()
    {

    }
}