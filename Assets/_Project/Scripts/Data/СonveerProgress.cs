using System;
using System.Collections.Generic;

[Serializable]
public class ÑonveerProgress
{
    public List<ConveerLineProgress> ConveerLines;

    public ÑonveerProgress()
    {
        ConveerLines = new List<ConveerLineProgress>()
        {
            new ConveerLineProgress(ConveerNumbers.Conveyor_1, true),
            new ConveerLineProgress(ConveerNumbers.Conveyor_2),
            new ConveerLineProgress(ConveerNumbers.Conveyor_3),
            new ConveerLineProgress(ConveerNumbers.Conveyor_4),
            new ConveerLineProgress(ConveerNumbers.Conveyor_bonus)
        };
    }
}
