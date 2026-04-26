using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameFactory : IGameFactory
{
    private GameObject _characterObject;

    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly ISaveLoadService _saveLoadService;
    private readonly IInputService _inputService;
    private readonly IConveerUpdaterService _conveerUpdateService;
    private readonly IPoolService _poolService;
    private readonly IWaypointChangerService _waypointsChanger;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IDifficultyService _difficultyService;

    public GameFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService,
        ISavedLists savedLists, ISaveLoadService saveLoadService, IInputService inputService, IConveerUpdaterService conveerUpdateService,
        IPoolService poolService, IWaypointChangerService waypointsChanger, ICoroutineRunner coroutineRunner,
        IDifficultyService difficultyService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _savedLists = savedLists;
        _saveLoadService = saveLoadService;
        _inputService = inputService;
        _conveerUpdateService = conveerUpdateService;
        _poolService = poolService;
        _waypointsChanger = waypointsChanger;
        _coroutineRunner = coroutineRunner;
        _difficultyService = difficultyService;
    }

    public CinemachineFreeLook CreateCamera()
    {
        CinemachineFreeLook camera = _assets.Instantiate(AssetsPath.CameraPath).GetComponent<CinemachineFreeLook>();
        camera.GetComponent<CinemachineZoom>().Construct(_inputService);
        camera.GetComponent<CameraRotateOnRightClick>().Construct(_inputService);

        return camera;
    }

    public Build CreateBuild(Transform at, LevelStaticData levelStaticData)
    {
        Build build = InstantiateRegistered(AssetsPath.BuildPath, at.position).GetComponent<Build>();

        build.FloorOpener.Construct(_saveLoadService, _progressService, levelStaticData);
        build.WallOpener.Construct(_saveLoadService, _progressService, levelStaticData);

        return build;
    }

    public Character CreateCharacterController(Transform at)
    {
        _characterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);

        Character controller = _characterObject.GetComponent<Character>();

        controller.Construct(_inputService, _difficultyService, _staticData.ForCharacterView(_progressService.Progress.PlayerState.CharacterViewType), at);

        return controller;
    }

    public LootPiece CreateLoot()
    {
        var lootPiece = InstantiateRegistered(AssetsPath.Loot)
            .GetComponent<LootPiece>();

        lootPiece.Construct(_progressService.Progress.PlayerState, _saveLoadService, _poolService);

        return lootPiece;
    }

    public Bullet CreateBullet()
    {
        GameObject bullet = _assets.Instantiate(AssetsPath.BulletPath);
        Bullet component = bullet.GetComponent<Bullet>();
        component.Construct(_poolService);

        return component;
    }

    public ATM CreateATM(Transform at)
    {
        ATM atm = _assets.Instantiate(AssetsPath.ATMPath, at.position)
            .GetComponent<ATM>();

        atm.Construct(_progressService, _saveLoadService);

        return atm;
    }

    public Enemy CreateMonster(MonsterTypeId typeID)
    {
        MonsterStaticData monsterData = _staticData.ForMonster(typeID);

        Enemy monster = Object.Instantiate(monsterData.Prefab)
            .GetComponent<Enemy>();

        monster.Construct(monsterData, _poolService, _waypointsChanger, _progressService, _saveLoadService);
        monster.HealthUI.Construct(monster.EnemyHealth);
        monster.EnemyHealth.Construct(monsterData.Health);
        monster.NavMeshAgent.speed = monsterData.MoveSpeed;
        monster.LootSpawner.Construct(this, _poolService);
        monster.LootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
        monster.Attack.Construct(monsterData.Damage, monsterData.EffectiveDistance,
            monsterData.Cleavage, monsterData.CooldownAttack);

        return monster;
    }

    public Waypoint CreateWaypoint(Vector3 at)
    {
        Waypoint waypoint = _assets.Instantiate(AssetsPath.WaypointPath)
            .GetComponent<Waypoint>();

        waypoint.transform.position = at;
        _waypointsChanger.AddNewPoint(waypoint);

        return waypoint;
    }

    public SpawnPoint CreateSpawner(Vector3 at, string id, MonsterTypeId monsterTypeId)
    {
        SpawnPoint spawner = InstantiateRegistered(AssetsPath.Spawner)
            .GetComponent<SpawnPoint>();

        spawner.Construct(this, _poolService);
        spawner.Id = id;
        spawner._monsterTypeID = monsterTypeId;
        spawner.transform.position = at;

        return spawner;
    }

    public ArsenalInitializer CreateArsenal(Transform at, LevelStaticData levelData, IIndicateUI indicator, IWindow arsenalWindow)
    {
        ArsenalInitializer arsenal = InstantiateRegistered(AssetsPath.Arsenal, at.transform.position).GetComponent<ArsenalInitializer>();
        arsenal.ArsenalView.Construct(indicator, arsenalWindow);
        arsenal.Opener.Construct(_saveLoadService, _progressService, levelData);
        return arsenal;
    }

    public WeaponSelector CreateWeaponSelector(Character character) =>
         new WeaponSelector(character, this, _progressService);

    public Weapon CreateWeapon(WeaponProgress weaponProgress) =>
        new Weapon(_staticData.ForWeaponsData(weaponProgress.Type), weaponProgress, _poolService, _assets, _coroutineRunner);

    public ConveerInitializer CreateConveerView(Transform at, ATM atm, IIndicateUI indicator, IWindow conveerShopWindow)
    {
        ConveerInitializer conveerInitializer = InstantiateRegistered(AssetsPath.ConveerPath, at.position).GetComponent<ConveerInitializer>();
        conveerInitializer.Construct(_staticData, _conveerUpdateService, _poolService);
        conveerInitializer.ConveerView.Construct(indicator, conveerShopWindow);
        conveerInitializer.Finisher.Construct(atm);
        return conveerInitializer;
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
