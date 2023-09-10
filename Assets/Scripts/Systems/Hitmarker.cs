using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    [SerializeField] private Color _hitColor;
    [SerializeField] private Color _destroyColor;
    [SerializeField] private float _hitmarkerDuration;

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
        await Awaitable.WaitForSecondsAsync(_hitmarkerDuration);
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
