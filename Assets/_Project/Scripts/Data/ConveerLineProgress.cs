using System;

[Serializable]
public class ConveerLineProgress
{
    public ConveerNumbers Numbers;
    public bool IsOpened;
    public int CooldownLevel;
    public int Level;

    public ConveerLineProgress(ConveerNumbers numbers, bool isOpened = false)
    {
        Numbers = numbers;
        IsOpened = isOpened;
        CooldownLevel = 1;
        Level = 1;
    }
}
