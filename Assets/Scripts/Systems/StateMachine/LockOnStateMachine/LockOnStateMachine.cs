using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class LockOnStateMachine : FiniteStateMachine
{
    public float Radius, Speed, Duration;
    public float MinDistance, MaxDistance, MinSize, MaxSize;
    public Color LockingOnColor, LockedOnColor;
    public EnemyUnit Target => _target;
    private EnemyUnit _target;
    private Image _image;
    private RectTransform _rectTransform;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;

        _rectTransform = GetComponent<RectTransform>();

        Init
        (
            new List<State>()
            {
                new NoTargetState(this),
                new LockingOnState(this),
                new LockedOnState(this)
            },
        0);
    }

    public void RemoveTarget() => _target = null;

    public void SetTarget()
    {
        EnemyUnit enemyUnit = null;
        float winnerDistance = 1f;
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
            if (WithinCenteredRadius(viewportPos))
            {
                float distance = Vector2.Distance(viewportPos, new Vector2(0.5f, 0.5f));

                if (distance < winnerDistance)
                {
                    winnerDistance = distance;
                    enemyUnit = enemy;
                }
            }
        }
        _target = enemyUnit;      
    }

    bool WithinCenteredRadius(Vector3 viewportPos)
    {
        return viewportPos.x > 0.5f - Radius &&
            viewportPos.x < 0.5f + Radius &&
            viewportPos.y > 0.5f - Radius &&
            viewportPos.y < 0.5f + Radius &&
            viewportPos.z > MinDistance && viewportPos.z < MaxDistance;
    }

    public void UpdateMarker(float lockOnCompletion)
    {
        _image.enabled = true;
        Vector3 worldPos = _target.transform.position;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        _rectTransform.anchoredPosition = viewportPos * new Vector2(Screen.width, Screen.height);
        _rectTransform.sizeDelta = GetMarkerSize(Vector3.Distance(Camera.main.transform.position, worldPos));
        _image.fillAmount = lockOnCompletion;
        _image.color = lockOnCompletion >= 1f ? LockedOnColor : LockingOnColor;
    }    

    Vector2 GetMarkerSize(float distance)
    {
        float t = distance / MaxDistance;
        float size = Mathf.Lerp(MaxSize, MinSize, t);
        return new Vector2(size, size);
    }
}
