using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgress
{
    public WorldData WorldData;
    public PlayerState PlayerState;
    public State CharacterState;
    public Arsenal Arsenal;
    public ÑonveerProgress Conveers;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        PlayerState = new PlayerState();
        CharacterState = new State();
        Arsenal = new Arsenal();
        Conveers = new ÑonveerProgress();
    }
}

[Serializable]
public class PlayerState
{
    public CharacterType CurrentType;
    public int Score;
    public int Cash;

    public PlayerState()
    {
        CurrentType = CharacterType.Sad;
        Score = 0;
        Cash = 100;
    }

    public event Action ValueChanged;

    public void Collect(Loot loot)
    {
        Cash += loot.Value;
        ValueChanged?.Invoke();
    }
}

[Serializable]
public class State
{
    public float CurrentHP;
    public float MaxHP;

    public void ResetHP() => CurrentHP = MaxHP;
}

[Serializable]
public class Arsenal
{
    public bool IsUnblocked = false;

    public Arsenal()
    {
        Weapons = new();
    }

    public List<OpenedWeapon> Weapons;

    public event Action<WeaponType> AddedNewWeapon;

    public OpenedWeapon AddWeapon(WeaponType type)
    {
        var weapon = new OpenedWeapon(type);

        if (!Weapons.Contains(weapon))
        {
            Weapons.Add(weapon);
            AddedNewWeapon?.Invoke(type);
        }

        return weapon;
    }
}

[Serializable]
public class OpenedWeapon
{
    private const int MaxLevel = 6;

    public WeaponType Type;
    public int MagazineLevel;
    public int DamageLevel;

    public OpenedWeapon(WeaponType type)
    {
        Type = type;
        MagazineLevel = 1;
        DamageLevel = 1;
    }

    public void UpdateMagazineLevel()
    {
        if (MagazineLevel < MaxLevel)
            MagazineLevel++;
    }

    public void UpdateDamageLevel()
    {
        if (DamageLevel < MaxLevel)
            DamageLevel++;
    }
}
