using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LockOnStateMachine : FiniteStateMachine
{
    public float Radius, Speed, Duration;
    public float MinDistance, MaxDistance, MinSize, MaxSize;
    public Color LockingOnColor, LockedOnColor;

    [HideInInspector] public float LockOnCompletion;
    [HideInInspector] public EnemyUnit Target;

    private Image _image;
    private RectTransform _rectTransform;
    private Awaitable _focusingDurationAwaitable;


    void Start()
    {
        Init
        (
            new List<State>()
            {
                new NoTargetState(this),
                new LockingOnState(this)
            },
        0);
    }
}
