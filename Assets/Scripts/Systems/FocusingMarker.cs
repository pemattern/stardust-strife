using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

 [RequireComponent(typeof(Image))]
public class FocusingMarker : MonoBehaviour
{
    [SerializeField] private float _focusingRadius, _focusingSpeed, _focusingDuration;
    [SerializeField] private float _minDistance, _maxDistance, _minSize, _maxSize;
    [SerializeField] private Color _targetingColor, _targetAquiredColor;
    private Image _image;
    private RectTransform _rectTransform;
    private EnemyUnit _currentTarget;
    private EnemyUnit _attemptingToTarget;
    private Task _focusingDurationTask;
    private float _targetingCompletion;

    public static FocusingMarker Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _focusingDurationTask = Task.Delay(0);
    }

    void Update()
    {
        if (NoTarget() && TryGetTarget(out EnemyUnit enemy)) _attemptingToTarget = enemy;

        if (TargetEnd()) ResetTargets();

        if (TargetAquired()) { _currentTarget = _attemptingToTarget; EnemyManager.SetTargetEnemy(_attemptingToTarget); }

        if (Targeting()) _targetingCompletion += _focusingSpeed * Time.deltaTime;
    
        if (TargetAquired() || Targeting())
        {
            if (OutOfViewport(Camera.main.WorldToViewportPoint(_attemptingToTarget.transform.position)))
            {
                ResetTargets();
                _image.enabled = false;
                return;
            }
            UpdateFocusingMarker(_attemptingToTarget);
        }
        else _image.enabled = false;
    }

    void UpdateFocusingMarker(EnemyUnit enemy)
    {
        _image.enabled = true;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        _rectTransform.anchoredPosition = viewportPos * new Vector2(Screen.width, Screen.height);
        _rectTransform.sizeDelta = GetMarkerSize(Vector3.Distance(Camera.main.transform.position, enemy.transform.position));
        _image.fillAmount = _targetingCompletion;
        if (TargetAquired()) _image.color = _targetAquiredColor;
        else _image.color = _targetingColor;
    }

    bool NoTarget() => _attemptingToTarget is null && _currentTarget is null;
    bool Targeting() => _attemptingToTarget is not null && _currentTarget is null && _targetingCompletion < 1f;
    bool TargetAquired() => _targetingCompletion >= 1f && _attemptingToTarget is not null;
    bool TargetEnd() => _focusingDurationTask.IsCompleted && _currentTarget is not null && !WithinCenteredRadius(_currentTarget.transform.position);

    bool OutOfViewport(Vector3 viewportPos)
    {
        return viewportPos.x < 0f ||
            viewportPos.x > 1f ||
            viewportPos.y < 0f ||
            viewportPos.y > 1f ||
            viewportPos.z < _minDistance;
    }

    bool WithinCenteredRadius(Vector3 viewportPos)
    {
        return viewportPos.x > 0.5f - _focusingRadius &&
            viewportPos.x < 0.5f + _focusingRadius &&
            viewportPos.y > 0.5f - _focusingRadius &&
            viewportPos.y < 0.5f + _focusingRadius &&
            viewportPos.z > _minDistance && viewportPos.z < _maxDistance;
    }

    public static void Reset()
    {
        Instance.ResetTargets();
    }

    Vector2 GetMarkerSize(float distance)
    {
        float t = distance / (_maxDistance);
        float size = Mathf.Lerp(_maxSize, _minSize, t);
        return new Vector2(size, size);
    }

    bool TryGetTarget(out EnemyUnit enemyUnit)
    {
        enemyUnit = null;
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

        if (enemyUnit is not null)
        {
            _focusingDurationTask = Task.Delay((int)(_focusingDuration * 1000));
            return true;
        }

        return false;        
    }

    void ResetTargets()
    {
        _attemptingToTarget = null;
        _currentTarget = null;
        _targetingCompletion = 0f;
        EnemyManager.SetTargetEnemy(null);
    }
}
