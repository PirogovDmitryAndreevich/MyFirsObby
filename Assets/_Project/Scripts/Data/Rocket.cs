using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _enemyMask;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void OnHit(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _enemyMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IHealth damageable))
            {
                damageable.TakeDamage(Damage);
            }
        }
    }
}