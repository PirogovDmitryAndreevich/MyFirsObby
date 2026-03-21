using System;
using Unity.VisualScripting;
using UnityEngine;
using YG;

internal class LoadSceneState : IPayloaderState<string>
{    
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain curtain, IGameFactory gameFactory,
        IPersistentProgressService progressService)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
        _progressService = progressService;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _gameFactory.Cleanup();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
        _curtain.Hide();

    private void OnLoaded()
    {
        InitEnemySpawner();

        InitGameWorld();
        InformProgressReaders();
        _stateMachine.Enter<GameLoopState>();
    }

    private void InitEnemySpawner()
    {
        foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
        {
            EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
        }
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)        
            progressReader.LoadProgress(_progressService.Progress);        
    }

    private void InitGameWorld()
    {
        GameObject character = _gameFactory.CreateCharacter(at: GameObject.FindWithTag(InitialPointTag));
        
        InitHud(character);

        CameraFollow(character);
    }

    private void InitHud(GameObject character)
    {
        GameObject hud;

        if (YG2.envir.isMobile)
            hud = _gameFactory.CreateHudMobile();
        else
            hud = _gameFactory.CreateHudDesktop();

            hud.GetComponentInChildren<ActorUI>().Construct(character.GetComponent<CharacterHealth>());
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
