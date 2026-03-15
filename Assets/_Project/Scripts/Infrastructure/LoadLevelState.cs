using System;

internal class LoadLevelState : IPayloaderState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter(string sceneName) => 
        _sceneLoader.Load(sceneName);

    public void Exit()
    {
        throw new NotImplementedException();
    }
}