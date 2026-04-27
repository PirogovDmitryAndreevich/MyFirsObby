using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
public class LevelStaticData : ScriptableObject
{
    [Header("Game Loop Settings")]
    public float EnemySpawningTimer;
    public float BossSpawningTimer;
    public float TimeBetweenBoss;
    public int MaxEnemiesOnScene;
    public float LittleNoobPercent;
    public int ToRandomEnemyScore;
    public int ToBossScore;
    public int ScoreToUpdateMultiplier;
    public int MultiplierChangeValue;
    public float DecreaseDelay;

    [Header("Game Settings")]
    public int UnblockArsenalPrice;
    public int UnblockWallPrice;
    public int UnblockFloorPrice;

    public string LevelKey;

    public List<EnemySpawnerData> EnemySpawners;

    public List<EnemyWaypointData> EnemyWaypoints;
}
