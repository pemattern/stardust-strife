using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    [SerializeField] private int _size, _seed;
    [SerializeField, Range(0, 1)] private float _cutoff;
    [SerializeField] private float _spread;

    private int[,,] _noiseValues;

    [SerializeField] private ComputeShader _computeShader;
    [SerializeField] private GameObject _asteroidPrefab;

    public void Init(Vector3 position, Quaternion rotation)
    {
        _noiseValues = new int[_size, _size, _size];

        using (ComputeBuffer computeBuffer = new ComputeBuffer(_size * _size * _size, sizeof(int)))
        {
            int kernel = _computeShader.FindKernel("SpawnAsteroid");
            _computeShader.SetInt("Size", _size);
            _computeShader.SetInt("Seed", _seed);
            _computeShader.SetFloat("Cutoff", _cutoff);
            _computeShader.SetBuffer(kernel, "Asteroids", computeBuffer);
            _computeShader.Dispatch(kernel, 4, 4, 4);

            computeBuffer.GetData(_noiseValues);
            computeBuffer.Release();
        }

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                if (_noiseValues[x, y, 0] == 1)
                {
                    var go = Instantiate(_asteroidPrefab);
                    go.transform.localPosition = new Vector3(x, y, 0) * _spread + Random.insideUnitSphere * _spread;
                    go.transform.parent = gameObject.transform;
                }
            }
        }
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }
}