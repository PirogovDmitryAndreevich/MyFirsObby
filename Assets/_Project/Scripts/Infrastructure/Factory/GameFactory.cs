using UnityEngine;
using UnityEngine.AI;

public class GameFactory : IGameFactory
{
    private GameObject _characterObject;

    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly ISaveLoadService _saveLoadService;

    public GameFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService,
        ISavedLists savedLists, ISaveLoadService saveLoadService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _savedLists = savedLists;
        _saveLoadService = saveLoadService;
    }

    public GameObject CreateCharacter(GameObject at)
    {
        _characterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);

        return _characterObject;
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

        lootPiece.Construct(_progressService.Progress.Resources, _saveLoadService);

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
        monster.GetComponent<AgentMoveToPlayer>().Construct(_characterObject.transform);
        monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

        LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
        lootSpawner.Construct(this);
        lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MixLoot);

        monster.GetComponent<Attack>()
            .Construct(_characterObject.transform, monsterData.Damage, monsterData.EffectiveDistance,
            monsterData.Cleavage, monsterData.CooldownAttack);

        monster.GetComponent<RotateToPlayer>()?.Construct(_characterObject.transform);
    }

    public void CreateSpawner(Vector3 at, string id, MonsterTypeID monsterTypeId)
    {
        SpawnPoint spawner = InstantiateRegistered(AssetsPath.Spawner)
            .GetComponent<SpawnPoint>();

        spawner.Construct(this);
        spawner.Id = id;
        spawner._monsterTypeID = monsterTypeId;
        spawner.transform.position = at;
    }

    private GameObject CreateHudMobile() =>
        InstantiateRegistered(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
        InstantiateRegistered(AssetsPath.HudDesktopPath);

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

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
    }

    private void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
        {
            if (!_savedLists.ProgressWriters.Contains(progressWriter))
                _savedLists.AddWriter(progressWriter);
        }

        if (!_savedLists.ProgressReaders.Contains(progressReader))
            _savedLists.AddReader(progressReader);
    }
}
