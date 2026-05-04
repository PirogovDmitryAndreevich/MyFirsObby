using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using YG;
using System;

internal class LoadSceneState : IPayloaderState<string>
{
    private const string BuildPointTag = "BuildInitialPoint";
    private const string LeaderBoardTag = "LeaderBoardInitialPoint";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IUIFactory _uiFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly IStaticDataService _staticData;

    private LoadedScene _loadedScene;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory, IUIFactory uIFactory,
        IPersistentProgressService progressService, ISavedLists savedLists,
        IStaticDataService staticData)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _uiFactory = uIFactory;
        _progressService = progressService;
        _savedLists = savedLists;
        _staticData = staticData;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _savedLists.Cleanup();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
        _curtain.Hide();

    private void OnLoaded()
    {
        InitGameWorld();        
        InformProgressReaders();
        _stateMachine.Enter<GameLoopState, LoadedScene>(_loadedScene);
    }

    private void InitGameWorld()
    {
        LevelStaticData levelData = GetLevelData();

        CreateLeaderBoard();

        IWeaponContainer container = CreateWeaponContainer();

        RewardAdvPopup rewardPopup = CreateRewardAdvPopup();

        List<SpawnPoint> enemySpawns = InitEnemySpawns(levelData);

        InitWaypoints(levelData);

        Character characterController =
            InitCharacter(at: levelData.PlayerInitialPoint);

        IWeaponSelector weaponSelector = InitWeaponSelector(characterController, container);

        Hud hud = InitHud(characterController.Health, characterController.Shooter, weaponSelector, container);

        Build build = InitBuild(levelData, rewardPopup, container);

        CameraFollow(characterController.CameraFollow);

        _loadedScene = new LoadedScene(levelData, characterController, enemySpawns, build, hud);
    }

    private IWeaponContainer CreateWeaponContainer() => 
        _gameFactory.CreateWeaponContainer();

    private RewardAdvPopup CreateRewardAdvPopup() =>
        _uiFactory.CreateRewardAdvPopup();

    private void CreateLeaderBoard() =>
        _gameFactory.CreateLeaderBoard(GameObject.FindWithTag(LeaderBoardTag).transform);

    private LevelStaticData GetLevelData()
    {
        string sceneKey = SceneManager.GetActiveScene().name;
        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        return levelData;
    }

    private Build InitBuild(LevelStaticData levelData, RewardAdvPopup rewardAdvPopup, IWeaponContainer container) =>
        _gameFactory.CreateBuild(at: GameObject.FindWithTag(BuildPointTag).transform, levelData, rewardAdvPopup, container);

    private Character InitCharacter(Vector3 at) =>
        _gameFactory.CreateCharacterController(at: at);

    private WeaponSelector InitWeaponSelector(Character character, IWeaponContainer container) =>
        _gameFactory.CreateWeaponSelector(character, container);

    private List<SpawnPoint> InitEnemySpawns(LevelStaticData levelData)
    {
        List<SpawnPoint> enemySpawns = new();

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        {
            SpawnPoint spawn = _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            enemySpawns.Add(spawn);
        }

        return enemySpawns;
    }
    private void InitWaypoints(LevelStaticData levelData)
    {
        foreach (EnemyWaypointData waypointData in levelData.EnemyWaypoints)
            _gameFactory.CreateWaypoint(at: waypointData.Position);
    }

    private Hud InitHud(CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector, IWeaponContainer container) =>
        _uiFactory.CreateHod(YG2.envir.isMobile, characterHealth, shooter, weaponSelector, container);

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _savedLists.ProgressReaders)
            progressReader.LoadProgress(_progressService.Progress);
    }

    private void CameraFollow(Transform cameraFollow)
    {
        CinemachineFreeLook camera = _gameFactory.CreateCamera();

        camera.Follow = cameraFollow;
        camera.LookAt = cameraFollow;
    }
}
