using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public MonsterTypeId _monsterTypeID;

    private IGameFactory _gameFactory;
    private IPoolService _poolService;

    public string Id { get; set; }

    public void Construct(IGameFactory gameFactory, IPoolService poolService)
    {
        _gameFactory = gameFactory;
        _poolService = poolService;

        _poolService.RegisterFactory(() => CreateLittleNoob());
        _poolService.RegisterFactory(() => CreateBigNoob());
        _poolService.RegisterFactory(() => CreateNoob());
    }

    public Enemy Spawn(MonsterTypeId typeId)
    {
        Enemy enemy = GetEnemy(typeId);
        Warp(enemy);
        enemy.ResetState();

        return enemy;
    }

    private Enemy GetEnemy(MonsterTypeId monsterTypeId)
    {
        switch (monsterTypeId)
        {
            case MonsterTypeId.Noob:
                return _poolService.Get<Noob>();
            case MonsterTypeId.LittleNoob:
                return _poolService.Get<LittleNoob>();
            case MonsterTypeId.NoobBoss:
                return _poolService.Get<BigNoob>();
            default:
                return null;
        }
    }

    private void Warp(Enemy enemy)
    {
        enemy.NavMeshAgent.enabled = false;
        enemy.transform.position = transform.position;
        enemy.NavMeshAgent.enabled = true;
    }

    private Noob CreateNoob() =>
        _gameFactory.CreateMonster(MonsterTypeId.Noob) as Noob;

    private LittleNoob CreateLittleNoob() =>
        _gameFactory.CreateMonster(MonsterTypeId.LittleNoob) as LittleNoob;

    private BigNoob CreateBigNoob() =>
        _gameFactory.CreateMonster(MonsterTypeId.NoobBoss) as BigNoob;
}
