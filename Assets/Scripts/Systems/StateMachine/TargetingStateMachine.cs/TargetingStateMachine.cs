using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TargetingStateMachince : FiniteStateMachine
{
    public float Radius, Speed, Duration;
    public float MinDistance, MaxDistance, MinSize, MaxSize;
    public float Color TargetingColor, TargetAquiredColor;
    private Image _image;
    private RectTransform _rectTransform;
    private EnemyUnit _currentTarget;
    private EnemyUnit _attemptingToTarget;
    private Awaitable _focusingDurationAwaitable;
    private float _targetingCompletion;

    public static FocusingMarker Instance;

    void Start()
    {
        Init
        (
            new List<State>()
            {
                new NoTargetState(this),
                new IncrementTargetState(this)
            },
        0);
    }
}
