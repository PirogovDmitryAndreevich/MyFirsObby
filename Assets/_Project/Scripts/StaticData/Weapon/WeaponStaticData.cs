using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
public class WeaponStaticData : ScriptableObject
{
    [Header("Type")]
    public WeaponType Type;

    [Header("View Settings")]
    public string Name;
    public int UnblockPrice;    
    public Sprite Icon;
    public GameObject Prefab;
    public Bullet BulletPrefab;

    [Header("Gun stats")]
    public int Damage = 5;
    public int MagazineSize = 10;
    public float Spread = 0f;
    public float ReloadTime = 0.3f;
    public int BulletsPerTap = 1;
    public float TimeBetweenShot = 0.2f;
    public float Distance = 200f;
}
