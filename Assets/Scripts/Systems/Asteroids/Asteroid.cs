using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 _randomVector;
    private float _moveSpeed, _rotationSpeed, _scale;
    private MeshRenderer[] _renderers;
    
    void Start()
    {
        _randomVector = Random.onUnitSphere;
        _moveSpeed = Random.Range(0.5f, 1.5f);
        _rotationSpeed = Random.Range(0.5f, 2.5f);
        _scale = Mathf.Pow(7, Random.Range(-1f, 1f));  
        transform.rotation = Random.rotation;
        transform.position += _randomVector;
        transform.localScale *= _scale;
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (_renderers.Any(x => x.isVisible == true))
        {
            transform.Rotate(_randomVector * Time.deltaTime * _rotationSpeed);
            transform.Translate(_randomVector * Time.deltaTime * _moveSpeed);
        }
    }
}  
