using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] private float _radius;
    [SerializeField] private ParticleSystem disableOnHit;
    [SerializeField] private RocketExplosion rocketExplosion;
    [SerializeField] private AudioSource _inFlightAudioSource;
    [SerializeField] private LayerMask _enemyMask;

    private AudioSettingSaveData _saveData;

    private void Start() =>
        _saveData = AllServices.Container.Single<IPersistentProgressService>().Progress.Audio;

    private void OnEnable()
    {
        if (_saveData != null)
        {
            SetAudioVolume();
        }
        else
        {
            _saveData = AllServices.Container.Single<IPersistentProgressService>().Progress.Audio;
            SetAudioVolume();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void OnHit(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _enemyMask);

        Explode();
        _inFlightAudioSource.Stop();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IHealth damageable))
            {
                damageable.TakeDamage(Damage);
            }
        }

        disableOnHit.Stop();
    }

    private void Explode()
    {
        RocketExplosion explode = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);
        explode.SetAudioVolume(_saveData.Volume, _saveData.IsMute);
    }

    private void SetAudioVolume()
    {
        _inFlightAudioSource.volume = _saveData.Volume;
        _inFlightAudioSource.mute = _saveData.IsMute;
    }
}