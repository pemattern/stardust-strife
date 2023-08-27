using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance;

    private RectTransform _rectTransform;
    [SerializeField] private RectTransform _childRectTransform;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public static void UpdateCrosshair(Vector3 playerPosition, Vector3 lookDirection, float projectileSpeed, float projectileLifetime)
    {
        Vector3 projectileVector = lookDirection * projectileSpeed * projectileLifetime;

        Vector3 projectileEndPoint = playerPosition + projectileVector;
        Instance._rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(projectileEndPoint);

        projectileEndPoint = playerPosition + projectileVector * 0.1f;
        Instance._childRectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(projectileEndPoint);
    }
}
