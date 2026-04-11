using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISavedProgressReader
{
    public MonsterTypeId _monsterTypeID;

    private IGameFactory _gameFactory;

    public string Id { get; set; }

    public void Construct(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        Spawn();
    }

    private void Spawn()
    {
        _gameFactory.CreateMonster(_monsterTypeID, transform);
    }
}
