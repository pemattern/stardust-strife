using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DamagedHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerUnit _player;
    private Image _healthBar;

    void Start()
    {  
        _healthBar = GetComponent<Image>();
        _player.Health.Decreased += HealthDecreased; 
        _player.Health.Increased += HealthIncreased;
    }

    async void HealthDecreased()
    {
        await Task.Delay(500);
        while (_player.Health.Normalized < _healthBar.fillAmount)
        {
            _healthBar.fillAmount -= Time.deltaTime;
            await Task.Yield();
        }
    }

    void HealthIncreased()
    {
        _healthBar.fillAmount = _player.Health.Normalized;
    }

    void OnDisable()
    {
        _player.Health.Decreased -= HealthDecreased;
        _player.Health.Increased -= HealthIncreased;
    }
}

