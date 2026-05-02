using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
public class LevelStaticData : ScriptableObject
{
    public Vector3 PlayerInitialPoint;
    public int StartATMCash;

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

    public string LevelKey;

    public List<EnemySpawnerData> EnemySpawners;

    public List<EnemyWaypointData> EnemyWaypoints;
}
