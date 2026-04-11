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

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);

        if (collision.collider.TryGetComponent(out IHealth damageable))
            damageable.TakeDamage(_damage);

        Destroy(gameObject);
    }

    public void Shot(Vector3 force, int damage)
    {
        _damage = damage;

        _rigidbody.velocity = force;        
    }
}