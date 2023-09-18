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
        _rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position);
        _healthBar.fillAmount = _health.Normalized;
        _shieldBar.fillAmount = _shield.Normalized;

        _background.enabled = !_displayAwaitable.IsCompleted;
        _healthBar.enabled = !_displayAwaitable.IsCompleted;
        _shieldBar.enabled = !_displayAwaitable.IsCompleted;

        if (InCenterScreen()) DisplayBar();
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