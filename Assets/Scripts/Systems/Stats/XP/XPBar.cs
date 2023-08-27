using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class XPBar : MonoBehaviour
{
    [SerializeField] private PlayerUnit _player;
    private Image _bar;

    void Start()
    {  
        _bar = GetComponent<Image>();
        _player.XP.Changed += UpdateBar; 
    }

    void UpdateBar()
    {
        _bar.fillAmount = _player.XP.Normalized;
    }

    void OnDisable()
    {
        _player.XP.Changed -= UpdateBar;
    }
}
