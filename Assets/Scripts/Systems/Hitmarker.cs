using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Hitmarker : MonoBehaviour
{
    [SerializeField] private Color _hitColor;
    [SerializeField] private Color _destroyColor;
    [SerializeField] private int _hitmarkerDuration = 50;

    private Image _image;
    private AudioSource _hitmarkerSound;

    void Start()
    {
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            enemy.Health.Decreased += DamagedHitmarker;
            enemy.Shield.Decreased += DamagedHitmarker;
            enemy.Health.ReachedZero += DestroyedHitmarker;
        }
        _image = GetComponent<Image>();
        _image.enabled = false;

        _hitmarkerSound = GetComponent<AudioSource>();
    }

    void DamagedHitmarker()
    {
        if (_image.enabled) return;
        ShowHitmarker(_hitColor);
    }

    void DestroyedHitmarker()
    {
        ShowHitmarker(_destroyColor);
    }

    async void ShowHitmarker(Color color)
    {
        _image.enabled = true;
        _image.color = color;
        _hitmarkerSound.Play();
        await Task.Delay(_hitmarkerDuration);
        _image.enabled = false;
    }

    void OnDisable()
    {
        foreach (EnemyUnit enemy in EnemyManager.Enemies)
        {
            enemy.Health.Decreased -= DamagedHitmarker;
            enemy.Shield.Decreased -= DamagedHitmarker;
            enemy.Health.ReachedZero -= DestroyedHitmarker;
        }        
    }
}
