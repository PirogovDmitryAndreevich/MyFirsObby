using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int _damage;
    private float _lifeTime = 5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage);

        Destroy(gameObject);
    }

    public void Shot(Vector3 velocity, int damage)
    {
        _rigidbody.velocity = velocity;
        _damage = damage;
    }
}