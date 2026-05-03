using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "StaticData/BuildData")]
public class BuildStaticData : ScriptableObject
{
    public List<ConveyerStaticData> Conveyers;
    public List<ForOpener> ForOpeners; 
}
