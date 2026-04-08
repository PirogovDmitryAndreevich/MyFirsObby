using System.Collections;

public class GameLoopState : IPayloaderState<LoadedScene>
{
    private readonly IPersistentProgressService _progressService;
    private readonly ICoroutineRunner _coroutineRunner;
    private LoadedScene _loadedScene;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ICoroutineRunner coroutineRunner)
    {
        _progressService = progressService;
        _coroutineRunner = coroutineRunner;
    }

    public void Enter(LoadedScene loadedScene)
    {
        _loadedScene = loadedScene;

        if (_progressService.Progress.PlayerState.IsTutorialCompleted)
            StartGameLoop();
        else        
            _coroutineRunner.StartCoroutine(StartTutorial());
    }

    public void Exit()
    {
    }

    private IEnumerator StartTutorial()
    {
        _loadedScene.Arsenal.Opener.SetActiveState(false);

        foreach(var line in _loadedScene.Conveer.Lines.Values)
            line.Opener.SetActiveState(false);

        ConveerLine line1 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_1];
        line1.Opener.SetActiveState(true);

        while (!line1.IsOpened) 
            yield return null;

        ArsenalInitializer arsenal = _loadedScene.Arsenal;
        arsenal.Opener.SetActiveState(true);

        while(!arsenal.IsOpened)
            yield return null;

        foreach (var line in _loadedScene.Conveer.Lines.Values)
        {
            if(!line.IsOpened)
                line.Opener.SetActiveState(true);
        }
    }

    private void StartGameLoop()
    {

    }
}
