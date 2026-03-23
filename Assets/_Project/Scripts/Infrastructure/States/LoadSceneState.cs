using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

internal class LoadSceneState : IPayloaderState<string>
{
    private const string InitialPointTag = "InitialPoint";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly ISavedLists _savedLists;
    private readonly IStaticDataService _staticData;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory,
        IPersistentProgressService progressService, ISavedLists savedLists,
        IStaticDataService staticData)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
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
        InitEnemySpawners();

        InitGameWorld();
        InformProgressReaders();
        _stateMachine.Enter<GameLoopState>();
    }

    private void InitGameWorld()
    {
        GameObject character = _gameFactory.CreateCharacter(at: GameObject.FindWithTag(InitialPointTag));

        InitHud(character);

        CameraFollow(character);
    }

    private void InitEnemySpawners()
    {
        string sceneKey = SceneManager.GetActiveScene().name;

        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private void InitHud(GameObject character)
    {
        GameObject hud = _gameFactory.CreateHod(YG2.envir.isMobile);

        hud.GetComponentInChildren<ActorUI>().Construct(character.GetComponent<CharacterHealth>());
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _savedLists.ProgressReaders)        
            progressReader.LoadProgress(_progressService.Progress);        
    }

    private void CameraFollow(GameObject character)
    {
        Transform cameraFollow = character.GetComponent<CharacterController>().CameraFollow;
        PlayerCamera playerCamera;

        if (Camera.main.TryGetComponent<PlayerCamera>(out playerCamera))
            playerCamera.SetFollowTransform(cameraFollow);
        else
            Camera.main
                .AddComponent<PlayerCamera>()
                .SetFollowTransform(cameraFollow);
    }
}
