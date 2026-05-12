using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DeathEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private EffectAudioHandler _audioHandler;
    [SerializeField] private AudioClip _effectSound;

    private IPoolService _pool;

    public EffectAudioHandler AudioHandler => _audioHandler;

    private void OnParticleSystemStopped() => 
        ReturnToPool();

    public void Construct(IPoolService pool) =>
        _pool = pool;

    public void Play()
    {
        _audioHandler.PlaySound(_effectSound);
        _particles.Play();
    }

    private void ReturnToPool() =>
        _pool.Return(this);
}
