using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject _fieldPrefab;
    [SerializeField] private GameObject _beltPrefab;

    [SerializeField] private int _minBelts, _maxBelts;

    void Start()
    {
        Instantiate(_fieldPrefab);

        Quaternion rotation = Random.rotation;
        for (int i = 0; i < Random.Range(_minBelts, _maxBelts + 1); i++)
        {
            AsteroidBelt asteroidBelt = Instantiate(_beltPrefab).GetComponent<AsteroidBelt>();
            asteroidBelt.Init(Random.insideUnitSphere * 500, rotation);
        }
    }
}