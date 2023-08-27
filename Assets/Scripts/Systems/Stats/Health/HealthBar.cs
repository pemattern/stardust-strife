using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerUnit _player;
    private Image _healthBar;

    void Start()
    {  
        _healthBar = GetComponent<Image>();
        _player.Health.Changed += UpdateBar; 
        _player.Health.Add(0);
    }

    void UpdateBar()
    {
        _healthBar.fillAmount = _player.Health.Normalized;
    }

    void OnDisable()
    {
        _player.Health.Changed -= UpdateBar;
    }
}
