using Unity.VisualScripting;
using UnityEngine;

internal class LoadLevelState : IPayloaderState<string>
{    
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _curtain = curtain;
        _gameFactory = gameFactory;
    }

    public void Enter(string sceneName)
    {
        _curtain.Show();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
        _curtain.Hide();

    private void OnLoaded()
    {
        GameObject character = _gameFactory.CreateCharacter(at: GameObject.FindWithTag(InitialPointTag));
        _gameFactory.CreateHud();

        CameraFollow(character);

        _stateMachine.Enter<GameLoopState>();
    }

    private void CameraFollow(GameObject character)
    {
        PlayerCamera playerCamera;

        if (Camera.main.TryGetComponent<PlayerCamera>(out playerCamera))
            playerCamera.SetFollowTransform(character.transform);
        else
            Camera.main
                .AddComponent<PlayerCamera>()
                .SetFollowTransform(character.transform);
    }    
}
