using System;
using System.Collections;

public class GameLoopState : IPayloaderState<LoadedScene>
{
    private const int MaxEnemiesOnScene = 4;

    private readonly IPersistentProgressService _progressService;
    private readonly ICoroutineRunner _coroutineRunner;
    private LoadedScene _loadedScene;
    private bool _isGameStarted;
    private int _enemiesOnScene;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ICoroutineRunner coroutineRunner)
    {
        _progressService = progressService;
        _coroutineRunner = coroutineRunner;
    }

    public void Enter(LoadedScene loadedScene)
    {
        _loadedScene = loadedScene;
        _isGameStarted = true;

        if (_progressService.Progress.PlayerState.IsTutorialCompleted)
            _coroutineRunner.StartCoroutine(StartGameLoop());
        else
            _coroutineRunner.StartCoroutine(StartTutorial());
    }

    public void Exit()
    {
    }

    private IEnumerator StartTutorial()
    {
        _loadedScene.Arsenal.Opener.SetActiveState(false);

        foreach (var line in _loadedScene.Conveer.Lines.Values)
            line.Opener.SetActiveState(false);

        ConveerLine line1 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_1];
        line1.Opener.SetActiveState(true);

        _loadedScene.Character.LineDraw.StartDraw(line1.Opener.transform);

        while (!line1.IsOpened)
            yield return null;

        _loadedScene.Character.LineDraw.StopDraw();

        ArsenalInitializer arsenal = _loadedScene.Arsenal;
        arsenal.Opener.SetActiveState(true);

        _loadedScene.Character.LineDraw.StartDraw(arsenal.Opener.transform);

        while (!arsenal.IsOpened)
            yield return null;

        _loadedScene.Character.LineDraw.StopDraw();

        var enemy = SpawnEnemy();

        _loadedScene.Character.LineDraw.StartDraw(enemy.transform);

        while(!enemy.EnemyDeath.IsDead)
            yield return null;

        _loadedScene.Character.LineDraw.StopDraw();

        _coroutineRunner.StartCoroutine(StartGameLoop());
    }

    private IEnumerator StartGameLoop()
    {
        foreach (var line in _loadedScene.Conveer.Lines.Values)
        {
            if (!line.IsOpened)
                line.Opener.SetActiveState(true);
        }

        while (_isGameStarted)
        {
            if (_enemiesOnScene < MaxEnemiesOnScene)
                SpawnEnemy();

            yield return null;
        }
    }

    private Enemy SpawnEnemy()
    {
        Random random = new Random();

        SpawnPoint spawner = _loadedScene.EnemySpawns[random.Next(_loadedScene.EnemySpawns.Count)];
        Enemy enemy = spawner.Spawn();
        enemy.EnemyDeath.Happened += EnemyDeath;
        _enemiesOnScene++;

        return enemy;
    }

    private void EnemyDeath(EnemyDeath enemyDeath)
    {
        enemyDeath.Happened -= EnemyDeath;
        _enemiesOnScene--;
    }
}
