using System;

[Serializable]
public class ConveerLineProgress
{
    public ConveerNumbers Numbers;
    public bool IsOpened;
    public int MaxCooldownLevel;
    public int MaxLevel;
    public int CurrentCooldownLevel;
    public int CurrentLevel;

    public event Action<ConveerUpdateType> ConveerLineUpdated;
    public event Action ConveerOpened;

    public ConveerLineProgress(ConveerNumbers numbers, ConveerStaticInfo staticData, bool isOpened = false)
    {
        Numbers = numbers;
        IsOpened = isOpened;
        MaxCooldownLevel = staticData.CooldownsInfo.Length;
        MaxLevel = staticData.LevelsInfo.Length;
        CurrentCooldownLevel = 1;
        CurrentLevel = 1;
    }

    public void Unblock()
    {
        IsOpened = true;
        ConveerOpened?.Invoke();
    }

    public void UpdateLevel(ConveerUpdateType type)
    {
        switch (type)
        {
            case ConveerUpdateType.Cooldawn:
                UpdateCooldownLevel();
                ConveerLineUpdated?.Invoke(type);
                break;
            case ConveerUpdateType.Level:
                UpdateLevel();
                ConveerLineUpdated?.Invoke(type);
                break;
        }

    }

    public bool CanUpgrade(ConveerUpdateType type)
    {
        int currentLevel = GetLevel(type);
        int maxLevel = GetMaxLevel(type);
        return currentLevel < maxLevel;
    }

    public int GetLevel(ConveerUpdateType type)
    {
        switch (type)
        {
            case ConveerUpdateType.Cooldawn:
                return CurrentCooldownLevel;
            case ConveerUpdateType.Level:
                return CurrentLevel;
            default:
                return 0;
        }
    }

    public int GetMaxLevel(ConveerUpdateType type)
    {
        switch (type)
        {
            case ConveerUpdateType.Cooldawn:
                return MaxCooldownLevel;
            case ConveerUpdateType.Level:
                return MaxLevel;
            default:
                return 0;
        }
    }

    private void UpdateCooldownLevel()
    {
        if (CurrentCooldownLevel < MaxCooldownLevel)
            CurrentCooldownLevel++;
    }

    private void UpdateLevel()
    {
        if (CurrentLevel < MaxLevel)
            CurrentLevel++;
    }
}
