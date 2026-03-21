using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;

    public List<ISavedProgressReader> ProgressReaders { get; } = new();
    public List<ISavedProgress> ProgressWriters { get; } = new();
    private  GameObject CharacterObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData)
    {
        _assets = assets;
        _staticData = staticData;
    }
    public void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
    }

    public void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
            ProgressWriters.Add(progressWriter);

        ProgressReaders.Add(progressReader);
    }

    public GameObject CreateCharacter(GameObject at)
    {
        CharacterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);

        return CharacterObject;
    }

    public GameObject CreateHudMobile() =>
        InstantiateRegistered(AssetsPath.HudMobilePath);

    public GameObject CreateHudDesktop() =>
        InstantiateRegistered(AssetsPath.HudDesktopPath);

    public void CreateMonster(MonsterTypeID typeID, Transform parent)
    {
        MonsterStaticData monsterData = _staticData.ForMonster(typeID);

        GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
        IHealth health = monster.GetComponent<IHealth>();

        health.Max = monsterData.Health;
        health.Current = monsterData.Health;

        monster.GetComponent<ActorUI>().Construct(health);
        monster.GetComponent<AgentMoveToPlayer>().Construct(CharacterObject.transform);
        monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
        monster.GetComponent<Attack>()
            .Construct(CharacterObject.transform, monsterData.Damage, monsterData.EffectiveDistance,
            monsterData.Cleavage, monsterData.CooldownAttack);

        monster.GetComponent<RotateToPlayer>()?.Construct(CharacterObject.transform);
    }

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath, at);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }    
}
