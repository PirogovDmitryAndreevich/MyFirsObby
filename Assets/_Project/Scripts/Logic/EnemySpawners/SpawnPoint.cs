using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISavedProgress
{
    public MonsterTypeID _monsterTypeID;

    private IGameFactory _gameFactory;

    public string Id { get; set; }

    public void Construct(IGameFactory gameFactory) =>
        _gameFactory = gameFactory;

    public void LoadProgress(PlayerProgress progress) => 
        Spawn();

    public void UpdateProgress(PlayerProgress progress)
    {
        
    }

    private void Spawn() => 
        _gameFactory.CreateMonster(_monsterTypeID, transform);
}
