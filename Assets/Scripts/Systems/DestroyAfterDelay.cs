using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float _delay;

    void Start()
    {
        Destroy(gameObject, _delay);    
    }
}
