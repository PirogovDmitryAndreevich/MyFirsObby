using System;
using UnityEngine;

public class EnemyDeathEffectSpawner : MonoBehaviour
{
    private IPoolService _pool;

    public void Construct(IPoolService pool, IGameFactory gameFactory)
    {
        _pool = pool;
        _pool.RegisterFactory(() => gameFactory.CreateEnemyDeathEffect());
    }

    public void StartDeathEffect()
    {
        DeathEffect effect = _pool.Get<DeathEffect>();
        effect.transform.position = transform.position;
        effect.Play();
    }
}
