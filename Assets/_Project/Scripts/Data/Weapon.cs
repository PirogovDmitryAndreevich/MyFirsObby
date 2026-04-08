using System;

[Serializable]
public class Weapon
{
    public WeaponType Type;

    public bool IsUnblocked { get; private set; }

    public int MagazineLevel;
    public int DamageLevel;
    public int MaxDamageLevel;
    public int MaxMagazineLevel;

    public bool IsMaxDamageLevel;
    public bool IsMaxMagazineLevel;

    public event Action<WeaponsUpdateType> WeaponsUpdate;
    public event Action StatusChanged;

    public Weapon(WeaponStaticData staticData)
    {
        Type = staticData.Type;
        MaxDamageLevel = staticData.DamageLevels.Levels.Length;
        MaxMagazineLevel = staticData.MagazineSizeLevels.Levels.Length;

        IsUnblocked = false;
        MagazineLevel = 1;
        DamageLevel = 1;
    }

    public void Unblock()
    {
        IsUnblocked = true;
        StatusChanged?.Invoke();
    }

    public void UpdateLevel(WeaponsUpdateType type)
    {
        switch (type)
        {
            case WeaponsUpdateType.MagazineSize:
                UpdateMagazineLevel();
                break;
            case WeaponsUpdateType.Damage:
                UpdateDamageLevel();
                break;
        }

        WeaponsUpdate?.Invoke(type);
    }

    public bool CanUpgrade(WeaponsUpdateType type)
    {
        int currentLevel = GetLevel(type);
        int maxLevel = GetMaxLevel(type);
        return currentLevel < maxLevel;
    }

    public int GetLevel(WeaponsUpdateType type)
    {
        switch (type)
        {
            case WeaponsUpdateType.MagazineSize:
                return MagazineLevel;
            case WeaponsUpdateType.Damage:
                return DamageLevel;
            default:
                return 0;
        }
    }

    public int GetMaxLevel(WeaponsUpdateType type)
    {
        switch (type)
        {
            case WeaponsUpdateType.MagazineSize:
                return MaxMagazineLevel;
            case WeaponsUpdateType.Damage:
                return MaxDamageLevel;
            default:
                return 0;
        }
    }

    private void UpdateMagazineLevel()
    {
        if (MagazineLevel < MaxMagazineLevel)        
            MagazineLevel++;
    }

    private void UpdateDamageLevel()
    {
        if (MagazineLevel < MaxDamageLevel)        
            DamageLevel++;
    }
}