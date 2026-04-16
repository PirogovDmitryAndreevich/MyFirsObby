using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
public class LevelStaticData : ScriptableObject
{
    public int UnblockArsenalPrice;
    public int UnblockWallPrice;
    public int UnblockFloorPrice;

    public string LevelKey;

    public List<EnemySpawnerData> EnemySpawners;

    public List<EnemyWaypointData> EnemyWaypoints;
}
