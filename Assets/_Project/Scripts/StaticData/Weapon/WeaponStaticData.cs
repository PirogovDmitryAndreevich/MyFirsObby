using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
public class WeaponStaticData : ScriptableObject
{
    public WeaponType Type;

    public string Name;
    public int UnblockPrice;
    public Sprite Icon;

    [Header("Setting")]
    public GameObject Prefab;
    public Bullet BulletPrefab;

    [Header("Gun stats")]
    public float Spread = 0f;

    public WeaponLevels MagazineSizeLevels;
    public WeaponLevels DamageLevels;

    public PriceWeaponLevels GetNextLevelInfo(int currentLevel, WeaponsUpdateType type)
    {
        int nextLevelIndex = currentLevel;

        return LevelInfo(nextLevelIndex, type);
    }

    public PriceWeaponLevels GeLevelInfo(int currentLevel, WeaponsUpdateType type)
    {
        int levelIndex = currentLevel - 1;
        return LevelInfo(levelIndex, type);
    }

    private PriceWeaponLevels LevelInfo(int index, WeaponsUpdateType type)
    {
        return type switch
        {
            WeaponsUpdateType.MagazineSize
                when index < MagazineSizeLevels.Levels.Length
            => MagazineSizeLevels.Levels[index],

            WeaponsUpdateType.Damage
                when index < DamageLevels.Levels.Length
            => DamageLevels.Levels[index],

            _ => null
        };
    }
}
