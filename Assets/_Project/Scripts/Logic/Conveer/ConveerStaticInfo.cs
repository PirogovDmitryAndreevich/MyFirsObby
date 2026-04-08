using System;

[Serializable]
public class ConveerStaticInfo
{
    public ConveerNumbers Number;

    public int UnblockPrice;

    public ConveerCooldownLevelPrice[] CooldownsInfo;
    public ConveerLevelsPrice[] LevelsInfo;

    public int GetNextLevelPrice(int currentLevel, ConveerUpdateType type)
    {
        switch (type)
        {
            case ConveerUpdateType.Cooldawn:
                return CooldownsInfo[currentLevel].Price;
            case ConveerUpdateType.Level:
                return LevelsInfo[currentLevel].Price;
            default:
                return 0;
        }
    }

    public ConveerLevelsPrice GetLevelInfo(int currentLevel)
    {
        int index = currentLevel - 1;
        return LevelsInfo[index];
    }

    public ConveerCooldownLevelPrice GetLevelCooldowInfo(int currentLevel)
    {
        int index = currentLevel - 1;
        return CooldownsInfo[index];
    }
}