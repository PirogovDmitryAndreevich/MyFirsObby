using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] private float _radius;
    [SerializeField] private ParticleSystem disableOnHit;
    [SerializeField] private GameObject rocketExplosion;
    [SerializeField] private AudioSource inFlightAudioSource;
    [SerializeField] private LayerMask _enemyMask;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void OnHit(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _enemyMask);

        Explode();
        inFlightAudioSource.Stop();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IHealth damageable))
            {
                damageable.TakeDamage(Damage);
            }
        }

        disableOnHit.Stop();
    }

    private void Explode() =>    
        Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);    
}