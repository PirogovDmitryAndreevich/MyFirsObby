using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoopState : IPayloaderState<LoadedScene>
{
    private readonly IPersistentProgressService _progressService;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly ITutorialCompleterService _tutorialCompleter;
    private readonly IDifficultyService _difficultyService;
    private readonly IOpenService _openService;
    private LoadedScene _loadedScene;
    private LevelStaticData _levelStaticData;
    private bool _isGameStarted;
    private int _enemiesOnScene;
    private float _timerBetweenBossTimer;
    private bool _isBossLife;
    private bool _isBossTimeStarted;
    private float _nextSpawnTime;

    public GameLoopState(GameStateMachine stateMachine, IPersistentProgressService progressService, ICoroutineRunner coroutineRunner,
        ITutorialCompleterService tutorialCompleter, IDifficultyService difficultyService, IOpenService openService)
    {
        _progressService = progressService;
        _coroutineRunner = coroutineRunner;
        _tutorialCompleter = tutorialCompleter;
        _difficultyService = difficultyService;
        _openService = openService;
    }

    public void Enter(LoadedScene loadedScene)
    {
        _loadedScene = loadedScene;
        _levelStaticData = _loadedScene.LevelData;
        _isGameStarted = true;
        _timerBetweenBossTimer = Time.time + _levelStaticData.TimeBetweenBoss;

        _difficultyService.Initialize(_loadedScene.LevelData);

        if (_progressService.Progress.PlayerState.IsTutorialCompleted)
            _coroutineRunner.StartCoroutine(StartGameLoop());
        else
            _coroutineRunner.StartCoroutine(StartTutorial());
    }

    public void Exit()
    {
        _isGameStarted = false;
    }

    private IEnumerator StartTutorial()
    {
        ATM atm = _loadedScene.Build.ATM;
        ArsenalInitializer arsenal = _loadedScene.Build.Arsenal;
        WeaponProgress weapon = _progressService.Progress.Arsenal.GetWeaponProgress(WeaponType.M1911);

        if (_progressService.Progress.PlayerState.Cash <= 0 && !_progressService.Progress.BuildProgress.OpenedObjects.Contains(OpeningType.Conveyor_1))
        {
            atm.AddCash(_levelStaticData.StartATMCash);

            _loadedScene.Character.LineDraw.StartDraw(atm.ATMButton.transform);

            while (atm.Cash > 0)
                yield return null;

            _openService.OpenOpener(OpeningType.Conveyor_1);

            _loadedScene.Character.LineDraw.StopDraw();
        }

        if (!_progressService.Progress.BuildProgress.OpenedObjects.Contains(OpeningType.Conveyor_1)
            && _progressService.Progress.BuildProgress.OpenedOpeners.Contains(OpeningType.Conveyor_1))
        { 
            IOpenObject opener = _openService.OpeningObjects[OpeningType.Conveyor_1];

            _loadedScene.Character.LineDraw.StartDraw(opener.Opener.transform);

            while(!_progressService.Progress.BuildProgress.OpenedObjects.Contains(OpeningType.Conveyor_1))
                yield return null;

            _loadedScene.Character.LineDraw.StopDraw();
        }

        if (!_progressService.Progress.BuildProgress.OpenedObjects.Contains(OpeningType.Arsenal)
            && _progressService.Progress.BuildProgress.OpenedOpeners.Contains(OpeningType.Arsenal))
        {
            IOpenObject arsenalOpener = _openService.OpeningObjects[OpeningType.Arsenal];

            _loadedScene.Character.LineDraw.StartDraw(arsenalOpener.Opener.transform);

            while (!_progressService.Progress.BuildProgress.OpenedObjects.Contains(OpeningType.Arsenal))
                yield return null;

            _loadedScene.Character.LineDraw.StopDraw();
        }

        if (!weapon.IsUnblocked)
        {
            _loadedScene.Character.LineDraw.StartDraw(arsenal.transform);

            while (!weapon.IsUnblocked)
                yield return null;

            _loadedScene.Character.LineDraw.StopDraw();
        }

        Enemy enemy = SpawnEnemy(MonsterTypeId.Noob);

        _loadedScene.Character.LineDraw.StartDraw(enemy.transform);

        while (!enemy.EnemyDeath.IsDead)
            yield return null;

        _loadedScene.Character.LineDraw.StopDraw();

        _tutorialCompleter.TutorialComplete();
        _coroutineRunner.StartCoroutine(StartGameLoop());
    }

    private IEnumerator StartGameLoop()
    {
        while (_isGameStarted)
        {
            SpawnLoopEnemy();
            SpawnLoopBoss();

            _difficultyService.CheckUpGameMultiplier();

            yield return null;
        }
    }

    private void SpawnLoopBoss()
    {
        if (!_isBossLife && !_isBossTimeStarted && Time.time >= _timerBetweenBossTimer
                        && _progressService.Progress.PlayerState.Score > _levelStaticData.ToBossScore)
        {
            _isBossTimeStarted = true;
            _coroutineRunner.StartCoroutine(StartBossTimer());
        }
    }

    private void SpawnLoopEnemy()
    {
        if (Time.time < _nextSpawnTime)
            return;

        if (_enemiesOnScene >= _levelStaticData.MaxEnemiesOnScene)
            return;

        SpawnEnemyInternal();

        _nextSpawnTime = Time.time + _levelStaticData.EnemySpawningTimer;
    }

    private void SpawnEnemyInternal()
    {
        MonsterTypeId type;

        if (_progressService.Progress.PlayerState.Score > _levelStaticData.ToRandomEnemyScore)
            type = GetRandomEnemy();
        else
            type = MonsterTypeId.Noob;

        var enemy = SpawnEnemy(type);

        _enemiesOnScene++;
    }

    private void SpawnBoss() =>
        SpawnEnemy(MonsterTypeId.NoobBoss);

    private MonsterTypeId GetRandomEnemy()
    {
        MonsterTypeId type;

        if (_levelStaticData.LittleNoobPercent >= Random.Range(0, 100))
            type = MonsterTypeId.LittleNoob;
        else
            type = MonsterTypeId.Noob;

        return type;
    }

    private IEnumerator StartBossTimer()
    {
        _loadedScene.Hud.BossTimer.ShowBossTimer();
        float timerToBoss = Time.time + _levelStaticData.BossSpawningTimer;

        while (timerToBoss > Time.time)
        {
            float timeLeft = timerToBoss - Time.time;
            _loadedScene.Hud.BossTimer.UpdateBossTimer(timeLeft);

            yield return null;
        }

        _isBossLife = true;
        _isBossTimeStarted = false;
        SpawnBoss();
        _loadedScene.Hud.BossTimer.HideBossTimer();
    }

    private Enemy SpawnEnemy(MonsterTypeId type)
    {
        SpawnPoint spawner = _loadedScene.EnemySpawns[Random.Range(0, _loadedScene.EnemySpawns.Count)];
        Enemy enemy = spawner.Spawn(type);
        enemy.EnemyDeath.Happened += EnemyDeath;

        return enemy;
    }

    private void EnemyDeath(EnemyDeath enemyDeath)
    {
        enemyDeath.Happened -= EnemyDeath;

        if (enemyDeath.TryGetComponent(out BigNoob _))
        {
            _isBossLife = false;
            _timerBetweenBossTimer = Time.time + _levelStaticData.TimeBetweenBoss;
        }
        else
        {
            _enemiesOnScene = Mathf.Max(0, _enemiesOnScene - 1);
        }
    }
}
