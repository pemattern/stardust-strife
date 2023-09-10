using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ShieldBar : MonoBehaviour
{
    [SerializeField] private PlayerUnit _player;
    private Image _shieldBar;

    void Start()
    {  
        _shieldBar = GetComponent<Image>();
        _player.Shield.Changed += UpdateBar; 
    }

    void UpdateBar()
    {
        _shieldBar.fillAmount = _player.Shield.Normalized;
    }

    void OnDisable()
    {
        _player.Shield.Changed -= UpdateBar;
    }
}
