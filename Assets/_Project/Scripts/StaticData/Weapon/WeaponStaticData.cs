using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
public class WeaponStaticData : ScriptableObject
{
    [Header("Type")]
    public WeaponType Type;
    public BulletType BulletType;

    [Header("View Settings")]
    public string Name;
    public int UnblockPrice;    
    public Sprite Icon;
    public WeaponBase Weapon;
    public AudioClip ShootSound;

    [Header("Gun stats")]
    public int Damage = 5;
    public int MagazineSize = 10;
    public float Spread = 0f;
    public float ReloadTime = 0.3f;
    public int BulletsPerTap = 1;
    public float TimeBetweenShot = 0.2f;
    public float Distance = 200f;
}
