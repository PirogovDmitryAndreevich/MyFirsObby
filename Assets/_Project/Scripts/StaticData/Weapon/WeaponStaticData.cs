using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
public partial class WeaponStaticData : ScriptableObject
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

        return type switch
        {
            WeaponsUpdateType.MagazineSize
                when nextLevelIndex < MagazineSizeLevels.Levels.Length
            => MagazineSizeLevels.Levels[nextLevelIndex],  

            WeaponsUpdateType.Damage
                when nextLevelIndex < DamageLevels.Levels.Length
            => DamageLevels.Levels[nextLevelIndex],

            _ => null
        };
    }
}
