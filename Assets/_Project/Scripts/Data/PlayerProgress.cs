using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgress
{
    public string MainScene = "Main";

    public PlayerState PlayerState;
    public Arsenal Arsenal;
    public ÑonveerProgress Conveers;

    public PlayerProgress(IStaticDataService staticData)
    {
        PlayerState = new PlayerState();
        Arsenal = new Arsenal(staticData);
        Conveers = new ÑonveerProgress();
    }
}

[Serializable]
public class PlayerState
{
    public CharacterType CurrentType;
    public int Score;
    public int Cash;
    public float CurrentHP;
    public float MaxHP;

    public PlayerState()
    {
        CurrentType = CharacterType.Sad;
        Score = 0;
        Cash = 500;
        MaxHP = 100;
        CurrentHP = MaxHP;
    }

    public event Action ValueChanged;

    public void ResetHP() => CurrentHP = MaxHP;

    public void AddCash(int cash)
    {
        Cash += cash;
        ValueChanged?.Invoke();
    }

    public void SpendCash(int cash)
    {
        Cash -= cash;
        ValueChanged?.Invoke();
    }
}

[Serializable]
public class Arsenal
{
    public bool IsUnblocked;
    public List<Weapon> Weapons;

    public event Action<Weapon> WeaponUnblocked;

    public Arsenal(IStaticDataService staticData)
    {
        IsUnblocked = false;
        Weapons = new();

        foreach (WeaponStaticData weaponStaticData in staticData.AllWeapons())
            Weapons.Add(new Weapon(weaponStaticData));
    }

    public void UnblockWeapon(Weapon weapon)
    {
        weapon.Unblock();
        WeaponUnblocked?.Invoke(weapon);

        /*foreach (Weapon currentWeapon in Weapons)
        {
            if (currentWeapon.Type == weapon.Type)
            {
                currentWeapon.Unblock();
                WeaponUnblocked?.Invoke(currentWeapon);
            }
        }*/
    }
}

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