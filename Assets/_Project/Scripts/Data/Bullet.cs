using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int _damage;
    private float _lifeTime = 5f;
    private IPoolService _poolService;

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

    public void Construct(IPoolService poolService) => 
        _poolService = poolService;

    public void Shot(Vector3 force, int damage)
    {
        _damage = damage;

        _rigidbody.velocity = force;
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

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _poolService.Return(this);
    }
}