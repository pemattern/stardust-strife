using UnityEngine;

public interface IUnitController
{
    public Vector3 Rotation { get; }
    public Vector3 Movement { get; }

    public bool Fire { get; }
    public bool AlternateFire { get; }
    public bool Boost { get; }
}