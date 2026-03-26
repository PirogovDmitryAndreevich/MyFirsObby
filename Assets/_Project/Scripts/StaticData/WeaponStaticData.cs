using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
public class WeaponStaticData : ScriptableObject
{
    public WeaponType Type;

    [Header("Setting")]
    public GameObject Prefab;
    public Bullet BulletPrefab;    

    [Header("Gun stats")]
    public float Spread = 0f;

    [Header("Magazine size")]
    public int[] MagazineLevels;

    [Header("Damage level")]
    public int[] DamageLevels;

    public int GetMagazineLength(int level)
    {
        if (level <= 0 || level > MagazineLevels.Length)
            return 0;

        return MagazineLevels[level - 1];
    }

    public int GetDamage(int level)
    {
        if (level <= 0 || level > DamageLevels.Length)
            return 0;

        return DamageLevels[level - 1];
    }
}