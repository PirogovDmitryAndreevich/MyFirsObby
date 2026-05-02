using System;
using System.Collections.Generic;

[Serializable]
public class BuildProgress
{
    public List<OpeningType> OpenedObjects;
    public List<OpeningType> OpenedOpeners;

    public BuildProgress()
    {
        OpenedObjects = new List<OpeningType>();
        OpenedOpeners = new List<OpeningType>();
    }
}