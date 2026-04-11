using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public MonsterTypeId _monsterTypeID;

    private IPoolService _poolService;

    public string Id { get; set; }

    public void Construct(IGameFactory gameFactory, IPoolService poolService)
    {
        _poolService = poolService;
        _poolService.RegisterFactory(() => gameFactory.CreateMonster(_monsterTypeID, transform));
    }

    public Enemy Spawn()
    {
        Enemy enemy = _poolService.Get<Enemy>();
        Warp(enemy);
        enemy.ResetState();

        return enemy;
    }

    private void Warp(Enemy enemy)
    {
        enemy.NavMeshAgent.enabled = false;
        enemy.transform.position = transform.position;
        enemy.NavMeshAgent.enabled = true;
    }
}
