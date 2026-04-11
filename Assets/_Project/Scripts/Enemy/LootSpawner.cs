using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private EnemyDeath _enemyDeath;
    private IGameFactory _gameFactory;
    private int _lootMin;
    private int _lootMax;

    private void Start() => 
        _enemyDeath.Happened += SpawnLoot;

    private void OnDestroy() => 
        _enemyDeath.Happened -= SpawnLoot;

    public void Construct(IGameFactory gameFactory) => 
        _gameFactory = gameFactory;

    public void SetLoot(int min, int max)
    {
        _lootMin = min;
        _lootMax = max;
    }

    private void SpawnLoot()
    {
        LootPiece loot = _gameFactory.CreateLoot();
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