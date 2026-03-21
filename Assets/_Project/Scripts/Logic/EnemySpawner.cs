using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISavedProgress
{
    [SerializeField] private MonsterTypeID _monsterTypeID;
    private IGameFactory _gameFactory;

    private void Awake()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();
    }

    public void LoadProgress(PlayerProgress progress)
    {
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        
    }

    private void Spawn()
    {
        _gameFactory.CreateMonster(_monsterTypeID, transform);
    }
}
