using System;
using System.Collections.Generic;

[Serializable]
public class ÑonveerProgress
{
    public List<ConveerLineProgress> ConveerLines;

    public ÑonveerProgress(IStaticDataService staticData)
    {
        ConveerLines = new List<ConveerLineProgress>();

        foreach (ConveerStaticInfo conveer in staticData.AllConveers())        
            ConveerLines.Add(new ConveerLineProgress(conveer.Number, conveer));        
    }
}
