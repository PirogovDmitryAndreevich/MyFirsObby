using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "StaticData/BuildData")]
public class BuildStaticData : ScriptableObject
{
    public List<ConveyerStaticData> Conveyers;
    public List<ForOpener> ForOpeners; 
}

[Serializable]
public class ConveyerStaticData
{
    public ConveyerType Type;
    public ConveerResource Resource;
}

[Serializable]
public enum ConveyerType
{
    gun,
    shotgun,
    ak74,
    rpg,
}

[Serializable]
public class ForOpener
{
    public OpeningType Type;
    public int UnblockPrice;
    public OpeningType[] Next;
}