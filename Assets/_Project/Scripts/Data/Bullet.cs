using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private BulletType _type;
    [SerializeField] private float _bulletForce;

    private Rigidbody _rigidbody;
    private int _damage;
    private float _lifeTime = 5f;
    private IBulletsPool _pool;

    public BulletType Type => _type;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IHealth damageable))
            damageable.TakeDamage(_damage);

        ReturnToPool();
    }

    public void Construct(IBulletsPool pool) => 
        _pool = pool;

    public void Shot(Vector3 force, int damage)
    {
        _damage = damage;

        transform.rotation = Quaternion.LookRotation(force);

        _trail.Clear();
        _trail.enabled = true;
        _rigidbody.velocity = force * _bulletForce;
        StartCoroutine(ReturnToPoolRoutine());
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        StopAllCoroutines();

        _trail.Clear();
        _trail.enabled = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _pool.Return(this);
    }
}