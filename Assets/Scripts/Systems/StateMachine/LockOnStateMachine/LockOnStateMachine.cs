using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class LockOnStateMachine : FiniteStateMachine, ITargetProvider
{
    public Unit Player;
    public float Radius, Speed;
    public float MinDistance, MaxDistance, MinSize, MaxSize;
    public Color LockingOnColor, LockedOnColor;
    public Unit Target => _target;
    private Unit _target;
    private Image _image;
    private RectTransform _rectTransform;
    private List<Weapon> _weapons;
    [HideInInspector] public bool LockingComplete;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;

        _rectTransform = GetComponent<RectTransform>();

        Weapon[] weapons = Player.GetComponents<Weapon>();
        _weapons = weapons.Where(x => x.RequiresLockOn == true).ToList();

        LockingComplete = false;

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

    public void Reset(Unit unit)
    {
        _target = null;
        _image.enabled = false;
        unit.Destroyed -= Reset;
        LockingComplete = false;
        SetWeaponLockState(true);
    }

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

    public void SetWeaponLockState(bool locked)
    {
        _weapons.ForEach(x => x.Locked = locked);
    }

    public bool OutOfViewport(Vector3 viewportPos)
    {
        return viewportPos.x < 0f ||
            viewportPos.x > 1f ||
            viewportPos.y < 0f ||
            viewportPos.y > 1f ||
            viewportPos.z < MinDistance;
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
