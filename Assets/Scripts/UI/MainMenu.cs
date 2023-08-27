using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<string> _menuItems;
    [SerializeField] private GameObject _prefab;
    void Start()
    {
        foreach (string menuItem in _menuItems)
        {
            CreateButton(menuItem, _prefab, transform);
        }
    }

    private void CreateButton(string text, GameObject prefab, Transform parent)
    {
        GameObject button = Instantiate(prefab, parent);
        Transform child = button.transform.GetChild(0);
        
        if (child.TryGetComponent(out TextMeshProUGUI tmPro))
            tmPro.text = text;
        else
            throw new MissingComponentException();
    }
}
