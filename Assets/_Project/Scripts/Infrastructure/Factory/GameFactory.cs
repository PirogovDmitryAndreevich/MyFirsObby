using Cinemachine;
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
    private readonly IPoolService _poolService;
    private readonly IWaypointChangerService _waypointsChanger;
    private readonly IWeaponCreator _weaponCreator;
    private readonly IDifficultyService _difficultyService;
    private readonly IOpenService _openService;
    private readonly IConveyersPool _conveyersPool;

    public GameFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService,
        ISavedLists savedLists, ISaveLoadService saveLoadService, IInputService inputService,
        IPoolService poolService, IWaypointChangerService waypointsChanger,IWeaponCreator weaponCreator,
        IDifficultyService difficultyService, IOpenService openService, IConveyersPool conveyersPool)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _savedLists = savedLists;
        _saveLoadService = saveLoadService;
        _inputService = inputService;
        _poolService = poolService;
        _waypointsChanger = waypointsChanger;
        _weaponCreator = weaponCreator;
        _difficultyService = difficultyService;
        _openService = openService;
        _conveyersPool = conveyersPool;
    }

    public CinemachineFreeLook CreateCamera()
    {
        CinemachineFreeLook camera = _assets.Instantiate(AssetsPath.CameraPath).GetComponent<CinemachineFreeLook>();
        camera.GetComponent<CinemachineZoom>().Construct(_inputService);
        camera.GetComponent<CameraRotateOnRightClick>().Construct(_inputService);

        return camera;
    }

    public Build CreateBuild(Transform at, LevelStaticData levelStaticData, RewardAdvPopup rewardPopup)
    {
        Build build = InstantiateRegistered(AssetsPath.BuildPath, at.position).GetComponent<Build>();

        build.ATM.Construct(_progressService, _saveLoadService);
        build.MoneyBugAdv.Construct(rewardPopup);

        foreach (var conveyer in build.Conveyers)
            conveyer.Construct(_conveyersPool, build.ATM);

        foreach (var stand in build.Stands)
            stand.Construct(_staticData, _weaponCreator, _saveLoadService);

        return build;
    }

    public Character CreateCharacterController(Vector3 at)
    {
        _characterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at);

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

    public WeaponSelector CreateWeaponSelector(Character character) =>
         new WeaponSelector(character, _weaponCreator);

    public void CreateLeaderBoard(Transform at)
    {
        LeaderBoard lb = _assets.Instantiate(AssetsPath.LeaderBoardPath, at.position)
            .GetComponent<LeaderBoard>();

        lb.Construct(_progressService);
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

        if (progressReader is IOpenObject openObject)
        {
            ForOpener data = _staticData.ForOpeners(openObject.Type);
            openObject.Opener.Construct(_openService,_progressService,data.Type,data.UnblockPrice);
            openObject.Construct(data.Next);
            _openService.Register(openObject);
        }

        if (!_savedLists.ProgressReaders.Contains(progressReader))
            _savedLists.AddReader(progressReader);
    }
}
