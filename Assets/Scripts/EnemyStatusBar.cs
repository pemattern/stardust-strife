using UnityEngine;
using UnityEngine.UI;

public class EnemyStatusBar : MonoBehaviour
{
    private Health _health;
    private Shield _shield;
    private Image _background;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _shieldBar;
    [SerializeField] private float _secondsDisplayed;
    [SerializeField] private float _centerRadius;
    [SerializeField] private Vector2 _offset;
    private EnemyUnit _enemy;
    private RectTransform _rectTransform;
    private Awaitable _displayAwaitable;
    public void Init(EnemyUnit enemy)
    {
        _enemy = enemy;
        _background = GetComponent<Image>();
        _health = enemy.GetComponent<Health>();
        _shield = enemy.GetComponent<Shield>();

        enemy.Destroyed += Dispose;
        enemy.Hit += DisplayBar;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _displayAwaitable = Awaitable.WaitForSecondsAsync(0f);
    }

    private void Update()
    {
        _rectTransform.anchoredPosition = GetPosition();
        _healthBar.fillAmount = _health.Normalized;
        _shieldBar.fillAmount = _shield.Normalized;

        _background.enabled = !_displayAwaitable.IsCompleted && InFrontOfPlayer();
        _healthBar.enabled = !_displayAwaitable.IsCompleted && InFrontOfPlayer();
        _shieldBar.enabled = !_displayAwaitable.IsCompleted && InFrontOfPlayer();

        if (InCenterScreen()) DisplayBar();
    }

    private Vector2 GetPosition()
    {
        Vector2 offset = _offset * Screen.currentResolution.height;
        return (Vector2)Camera.main.WorldToScreenPoint(_enemy.transform.position) + offset;
    }

    public void DisplayBar()
    {
        _displayAwaitable = Awaitable.WaitForSecondsAsync(_secondsDisplayed);
    }

    void Dispose(Unit unit)
    {
        _enemy.Destroyed -= Dispose;
        _enemy.Hit -= DisplayBar;
        Destroy(gameObject);
    }

    public bool InFrontOfPlayer()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(_enemy.transform.position);
        return viewportPos.z > 0f;
    }

    public bool InCenterScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(_enemy.transform.position);
        return viewportPos.x > 0.5f - _centerRadius &&
            viewportPos.x < 0.5f + _centerRadius &&
            viewportPos.y > 0.5f - _centerRadius &&
            viewportPos.y < 0.5f + _centerRadius &&
            viewportPos.z > 0f;
    }

    void OnDisable()
    {
        _enemy.Destroyed -= Dispose;
        _enemy.Hit -= DisplayBar;
    }
}