using System;
using System.Collections.Generic;

[Serializable]
public class ConveerProgress
{
    public List<ConveerLineProgress> ConveerLines;

    public ConveerProgress(IStaticDataService staticData)
    {
        ConveerLines = new List<ConveerLineProgress>();

        foreach (ConveerStaticInfo conveer in staticData.AllConveers())        
            ConveerLines.Add(new ConveerLineProgress(conveer.Number, conveer));        
    }
}
