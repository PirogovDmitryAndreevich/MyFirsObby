using System;

[Serializable]
public class PlayerState
{
    public CharacterType CharacterViewType;
    public bool IsTutorialCompleted;
    public int Score;
    public int Cash;
    public int Multiplier;
    public float CurrentHP;
    public float MaxHP;

    public PlayerState(IStaticDataService staticData)
    {
        CharacterStartData startData = staticData.GetCharacterStartData();

        CharacterViewType = startData.CharacterViewType;
        Score = startData.Score;
        Cash = startData.Cash;
        MaxHP = startData.MaxHP;
        CurrentHP = MaxHP;
        IsTutorialCompleted = false;
        Multiplier = 1;
    }

    public event Action CashChanged;
    public event Action ScoreChanged;
    public event Action MultiplierChanged;

    public void ResetHP() => CurrentHP = MaxHP;

    public void AddScore(int score)
    {
        Score += score;
        ScoreChanged?.Invoke();
    }

    public void AddCash(int cash)
    {
        Cash += cash;
        CashChanged?.Invoke();
    }

    public void SpendCash(int cash)
    {
        Cash -= cash;
        CashChanged?.Invoke();
    }

    public void CompleteTutorial() =>
        IsTutorialCompleted = true;

    public void ChangeMultiplier(int value) =>
        Multiplier = value;
}