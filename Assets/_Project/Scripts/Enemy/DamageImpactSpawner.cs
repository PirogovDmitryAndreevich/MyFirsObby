using UnityEngine;

public class DamageImpactSpawner : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;
    private IPoolService _poolService;

    private void Start() =>
        _health.DamageReceived += SpawnImpact;

    private void OnDestroy() =>
        _health.DamageReceived -= SpawnImpact;

    public void Construct(IGameFactory gameFactory, IPoolService poolService)
    {
        _poolService = poolService;
        _poolService.RegisterFactory(() => gameFactory.CreateDamageImpact());
    }

    private void SpawnImpact(float damage)
    {
        DamageImpactPiece piece = _poolService.Get<DamageImpactPiece>();

        piece.Initialize(damage, transform.position);
    }
}
