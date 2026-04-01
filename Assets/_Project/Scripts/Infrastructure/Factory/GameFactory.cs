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
    private readonly IInputService _inputService;

    public GameFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService,
        ISavedLists savedLists, ISaveLoadService saveLoadService, IInputService inputService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _savedLists = savedLists;
        _saveLoadService = saveLoadService;
        _inputService = inputService;
    }        

    public Character CreateCharacterController(Transform at)
    {
        _characterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);

        Character controller = _characterObject.GetComponent<Character>();

        controller.Construct(_inputService, _staticData.ForCharacterView(_progressService.Progress.PlayerState.CurrentType));

        return controller;
    }

    public LootPiece CreateLoot()
    {
        var lootPiece = InstantiateRegistered(AssetsPath.Loot)
            .GetComponent<LootPiece>();

        lootPiece.Construct(_progressService.Progress.PlayerState, _saveLoadService);

        return lootPiece;
    }

    public void CreateMonster(MonsterTypeID typeID, Transform parent)
    {
        MonsterStaticData monsterData = _staticData.ForMonster(typeID);

        GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
        EnemyHealth health = monster.GetComponent<EnemyHealth>();

        monster.GetComponent<HealthUI>().Construct(health);

        health.Construct(monsterData.Health);

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

    public ArsenalView CreateArsenal(Transform at, IIndicateUI indicator, IWindow arsenalWindow)
    {
        ArsenalView arsenal = InstantiateRegistered(AssetsPath.Arsenal, at.transform.position).GetComponent<ArsenalView>();
        arsenal.Construct(_progressService, indicator, arsenalWindow);
        return arsenal;
    }    

    public WeaponSelector CreateWeaponSelector(Character character)
    {
        WeaponSelector weaponSelector = new WeaponSelector();
        weaponSelector.Construct(character, _staticData, _progressService);

        return weaponSelector;
    }

    public GameObject CreateConveer(GameObject at) =>
        InstantiateRegistered(AssetsPath.ConveerPath, at.transform.position);

    public void CreateConveerManager(Conveer conveerComponent)
    {
        ConveerManager conveerManager = new ConveerManager(_saveLoadService, _staticData, conveerComponent);
        Register(conveerManager);
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

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgress progressReader in gameObject.GetComponentsInChildren<ISavedProgress>())
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
