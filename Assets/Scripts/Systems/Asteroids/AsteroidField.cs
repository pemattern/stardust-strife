using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] private int _size;
    [SerializeField, Range(0, 1)] private float _cutoff;
    [SerializeField] private float _spread;

    private int[,,] _noiseValues;

    [SerializeField] private ComputeShader _computeShader;
    [SerializeField] private GameObject _asteroidPrefab;

    void Start()
    {
        _noiseValues = new int[_size, _size, _size];

        using (ComputeBuffer computeBuffer = new ComputeBuffer(_size * _size * _size, sizeof(int)))
        {
            int kernel = _computeShader.FindKernel("SpawnAsteroid");
            _computeShader.SetInt("Size", _size);
            _computeShader.SetFloat("Cutoff", _cutoff);
            _computeShader.SetBuffer(kernel, "Asteroids", computeBuffer);
            _computeShader.Dispatch(kernel, 4, 4, 4);

            computeBuffer.GetData(_noiseValues);
            computeBuffer.Release();
        }

        int asteroidCount = 0;

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                for (int z = 0; z < _size; z++)
                {
                    if (_noiseValues[x, y, z] == 1)
                    {
                        var go = Instantiate(_asteroidPrefab);
                        go.transform.localPosition = new Vector3(x, y, z) * _spread + Random.insideUnitSphere * _spread;
                        go.transform.parent = gameObject.transform;
                        asteroidCount++;
                    }
                }
            }
        }
        gameObject.transform.position -= Vector3.one * _size;
    }
}