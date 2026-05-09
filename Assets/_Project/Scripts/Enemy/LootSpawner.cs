using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private EnemyDeath _enemyDeath;
    private IPoolService _poolService;
    private int _lootMin;
    private int _lootMax;
    private bool _isSpawned;

    private void Start() => 
        _enemyDeath.Happened += SpawnLoot;

    private void OnDestroy() => 
        _enemyDeath.Happened -= SpawnLoot;

    public void Construct(IGameFactory gameFactory, IPoolService poolService)
    {
        _poolService = poolService;
        _poolService.RegisterFactory(() => gameFactory.CreateLoot());
    }

    public void SetLoot(int min, int max)
    {
        _lootMin = min;
        _lootMax = max;
    }

    public void ResetState() =>
        _isSpawned = false;

    private void SpawnLoot(EnemyDeath _)
    {
        if (_isSpawned)
            return;

        _isSpawned = true;
        LootPiece loot = _poolService.Get<LootPiece>();
        loot.transform.position = transform.position;
        Loot lootItem = GenerateLoot();

        loot.Initialize(lootItem);
    }

    private Loot GenerateLoot()
    {
        return new Loot()
        {
            Value = Random.Range(_lootMin, _lootMax)
        };
    }
}