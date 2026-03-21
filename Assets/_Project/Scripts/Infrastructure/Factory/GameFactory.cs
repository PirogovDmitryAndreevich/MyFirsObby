using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private IPersistentProgressService _progressService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new();
    public List<ISavedProgress> ProgressWriters { get; } = new();
    private  GameObject CharacterObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
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

    public GameObject CreateHod(bool isMobile)
    {
        GameObject hod;

        if (isMobile)        
            hod = CreateHudMobile();        
        else        
            hod = CreateHudDesktop();

        hod.GetComponentInChildren<LootCounter>()
            .Construct(_progressService.Progress.Resources);

        return hod;
    }

    public LootPiece CreateLoot()
    {
        var lootPiece = InstantiateRegistered(AssetsPath.Loot)
            .GetComponent<LootPiece>();

        lootPiece.Construct(_progressService.Progress.Resources);

        return lootPiece;
    }

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

        LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
        lootSpawner.Construct(this);
        lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MixLoot);

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

    private GameObject CreateHudMobile() =>
        InstantiateRegistered(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
        InstantiateRegistered(AssetsPath.HudDesktopPath);

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
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
