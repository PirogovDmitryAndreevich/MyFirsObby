using UnityEngine;

public class DifficultyService : IDifficultyService
{
    private readonly ISaveLoadService _save;
    private readonly IPersistentProgressService _progressService;

    private LevelStaticData _levelStaticData;
    private int _characterDeathCounter;
    private int _lastProcessedStage;
    private float _lastProgressTime;

    public DifficultyService(ISaveLoadService save, IPersistentProgressService progressService)
    {
        _save = save;
        _progressService = progressService;
    }

    public void Initialize(LevelStaticData levelStaticData) =>
        _levelStaticData = levelStaticData;

    public void CheckUpGameMultiplier()
    {
        CheckUpDifficulty();
        CheckDecrease();
    }

    private void CheckUpDifficulty()
    {
        int score = _progressService.Progress.PlayerState.Score;
        int stage = score / _levelStaticData.ScoreToUpdateMultiplier;

        if (stage > _lastProcessedStage)
        {
            ApplyStage();
            _lastProcessedStage = stage;
            _characterDeathCounter = 0;

            _lastProgressTime = Time.time;

            _save.SaveProgress();
        }
    }

    public void CharacterDeath() =>
        _characterDeathCounter++;

    private void ApplyStage()
    {
        int multiplier = _progressService.Progress.PlayerState.Multiplier;

        multiplier += _levelStaticData.MultiplierChangeValue;

        _progressService.Progress.PlayerState.ChangeMultiplier(Mathf.Max(0, multiplier));
    }

    private void CheckDecrease()
    {
        if (Time.time - _lastProgressTime < _levelStaticData.DecreaseDelay)
            return;

        if (_characterDeathCounter == 0)
            return;

        int multiplier = _progressService.Progress.PlayerState.Multiplier;

        multiplier -= _levelStaticData.MultiplierChangeValue;

        _progressService.Progress.PlayerState.ChangeMultiplier(Mathf.Max(0, multiplier));

        _characterDeathCounter = 0;
        _lastProgressTime = Time.time;

        _save.SaveProgress();
    }
}