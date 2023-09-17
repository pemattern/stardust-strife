using UnityEngine;
using UnityEngine.UI;

public class EnemyStatusBar : MonoBehaviour
{
    private Health _health;
    private Shield _shield;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _shieldBar;
    private EnemyUnit _enemy;
    private RectTransform _rectTransform;

    public void Init(EnemyUnit enemy)
    {
        _enemy = enemy;
        _health = enemy.GetComponent<Health>();
        _shield = enemy.GetComponent<Shield>();

        enemy.Destroyed += Dispose;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _rectTransform.anchoredPosition = GetPosition();
        _healthBar.fillAmount = _health.Normalized;
        _shieldBar.fillAmount = _shield.Normalized;
    }

    private Vector2 GetPosition()
    {
        Vector2 halfRes = new Vector2(Screen.width, Screen.height) / 2;
        return (Vector2)Camera.main.WorldToScreenPoint(_enemy.transform.position) - halfRes;
    }

    void Dispose(Unit unit)
    {
        _enemy.Destroyed -= Dispose;
        Destroy(gameObject);
    }
}