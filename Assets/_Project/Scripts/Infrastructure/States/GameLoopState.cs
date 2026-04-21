using System;
using System.Collections;

public class GameLoopState : IPayloaderState<LoadedScene>
{
    private const int MaxEnemiesOnScene = 4;

    private readonly IPersistentProgressService _progressService;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly ITutorialCompleterService _tutorialCompleter;
    private LoadedScene _loadedScene;
    private bool _isGameStarted;
    private int _enemiesOnScene;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ICoroutineRunner coroutineRunner,
        ITutorialCompleterService tutorialCompleter)
    {
        _progressService = progressService;
        _coroutineRunner = coroutineRunner;
        _tutorialCompleter = tutorialCompleter;
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
        ArsenalInitializer arsenal = _loadedScene.Arsenal;
        arsenal.Opener.gameObject.SetActive(false);

        foreach (var line in _loadedScene.Conveer.Lines.Values)
            line.Opener.gameObject.SetActive(false);

        ConveerLine line1 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_1];

        if (!line1.IsOpened)
        {
            line1.Opener.gameObject.SetActive(true);
            _loadedScene.Character.LineDraw.StartDraw(line1.Opener.transform);

            while (!line1.IsOpened)
                yield return null;

            _loadedScene.Character.LineDraw.StopDraw();
        }

        if (!arsenal.IsOpened)
        {
            arsenal.Opener.gameObject.SetActive(true);
            _loadedScene.Character.LineDraw.StartDraw(arsenal.Opener.transform);

            while (!arsenal.IsOpened)
                yield return null;

            _loadedScene.Character.LineDraw.StopDraw();
        }

        var enemy = SpawnEnemy(MonsterTypeId.Noob);

        _loadedScene.Character.LineDraw.StartDraw(enemy.transform);

        while (!enemy.EnemyDeath.IsDead)
            yield return null;

        _loadedScene.Character.LineDraw.StopDraw();

        _tutorialCompleter.TutorialComplete();
        _coroutineRunner.StartCoroutine(StartGameLoop());
    }

    private IEnumerator StartGameLoop()
    {
        ConveerLine line2 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_2];
        ConveerLine line3 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_3];
        ConveerLine line4 = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_4];
        ConveerLine lineBonus = _loadedScene.Conveer.Lines[ConveerNumbers.Conveyor_bonus];

        if (!line2.IsOpened)
            line2.Opener.gameObject.SetActive(true);

        while (_isGameStarted)
        {
            if (line2.IsOpened && !line3.IsOpened)
                line3.Opener.gameObject.SetActive(true);

            if (line2.IsOpened && !_loadedScene.Build.IsWallOpened)
                _loadedScene.Build.WallOpener.gameObject.SetActive(true);

            if(_loadedScene.Build.IsWallOpened && !_loadedScene.Build.IsFloorOpened)
                _loadedScene.Build.FloorOpener.gameObject.SetActive(true);

            if (line3.IsOpened && !line4.IsOpened)
                line4.Opener.gameObject.SetActive(true);

            if (_enemiesOnScene < MaxEnemiesOnScene)
                SpawnEnemy(MonsterTypeId.Noob);                         ////////////////

            yield return null;
        }
    }

    private Enemy SpawnEnemy(MonsterTypeId type)
    {
        Random random = new Random();

        SpawnPoint spawner = _loadedScene.EnemySpawns[random.Next(_loadedScene.EnemySpawns.Count)];
        Enemy enemy = spawner.Spawn(type);
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
