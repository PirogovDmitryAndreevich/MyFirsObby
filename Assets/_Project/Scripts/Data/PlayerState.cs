using System;

[Serializable]
public class PlayerState
{
    public CharacterType CurrentType;
    public bool IsTutorialCompleted { get; private set; }
    public int Score;
    public int Cash;
    public float CurrentHP;
    public float MaxHP;

    public PlayerState()
    {
        CurrentType = CharacterType.Sad;
        Score = 0;
        Cash = 500;
        MaxHP = 100;
        CurrentHP = MaxHP;
        IsTutorialCompleted = false;
    }

    public event Action ValueChanged;

    public void ResetHP() => CurrentHP = MaxHP;

    public void AddCash(int cash)
    {
        Cash += cash;
        ValueChanged?.Invoke();
    }

    public void SpendCash(int cash)
    {
        Cash -= cash;
        ValueChanged?.Invoke();
    }

    public void CompleteTutorial() =>
        IsTutorialCompleted = true;
}