using UnityEngine;

[RequireComponent(typeof(Health))]
public class Shield : Stat
{
    private Health _health;
    private float _lastTimeDamaged;
    [SerializeField] private float _rechargeDelay, _rechargeSpeed, _shieldScale;
    [SerializeField] private Material _shieldMaterial;
    private GameObject _shieldObject;

    public void AddRechargeDelay(float amount)
    {
        _rechargeDelay += amount;
    }

    public void AddRechargeSpeed(float amount)
    {
        _rechargeSpeed += amount;
    }
    public override void Start()
    {
        base.Start();
        _health = GetComponent<Health>();

        Decreased += ShieldDamaged;
        ReachedZero += ShieldDestroyed;
        Increased += ShieldReactivated;
        Overkill += _health.Add;

        _shieldObject = CreateShieldObject();
    }

    void ShieldDamaged()
    {
        _lastTimeDamaged = Time.time;
    }

    void ShieldDestroyed()
    {
        _shieldObject.SetActive(false);
    }

    void ShieldReactivated()
    {
        if (!_shieldObject.activeInHierarchy) _shieldObject.SetActive(true);
    }

    void Update()
    {
        if (Time.time > _lastTimeDamaged + _rechargeDelay && Normalized < 1f)
            Add(_rechargeSpeed * Time.deltaTime);
    }

    GameObject CreateShieldObject()
    {
        GameObject shield = new GameObject("Shield");
        shield.transform.parent = gameObject.transform;
        shield.transform.localPosition = Vector3.zero;
        shield.transform.localRotation = Quaternion.identity;
        shield.transform.localScale = Vector3.one * _shieldScale;
        shield.AddComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;

        Material[] materials = new Material[gameObject.GetComponent<MeshRenderer>().materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = _shieldMaterial;
        }

        shield.AddComponent<MeshRenderer>().materials = materials;
        shield.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return shield;
    }

    void OnDisable()
    {
        Decreased -= ShieldDamaged;
        ReachedZero -= ShieldDestroyed;
        Increased -= ShieldReactivated;
        Overkill -= _health.Add;
    }
}