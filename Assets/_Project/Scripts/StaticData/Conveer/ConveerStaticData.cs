using UnityEngine;

[CreateAssetMenu(fileName = "ConveerData", menuName = "StaticData/Conveer")]
public class ConveerStaticData : ScriptableObject
{
    public readonly int MinLevel = 1;
    public readonly int MaxLevel = 8;

    public GameObject[] WeaponsPrefabs;
    public float[] CooldownLevels;

    public float GetLevelCooldown(int level)
    {
        if (level < MinLevel || level > MaxLevel)
            return 0;

        return CooldownLevels[level - 1];
    }

    public GameObject GetLevelPrefab(int level)
    {
        if (level < MinLevel || level > MaxLevel)
            return null;

        return WeaponsPrefabs[level - 1];
    }
}