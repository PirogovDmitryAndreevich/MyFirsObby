using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class PlayerProgress
{
    public WorldData WorldData;
    public PlayerResources Resources;
    public State CharacterState;
    public Arsenal Arsenal;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        Resources = new PlayerResources();
        CharacterState = new State();
        Arsenal = new Arsenal();
    }
}

[Serializable]
public class PlayerResources
{
    public int Score;
    public int Cash;
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

    public void AddWeapon(WeaponType type)
    {
        var weapon = new OpenedWeapon(type);

        if (!Weapons.Contains(weapon))
        {
            Weapons.Add(weapon);
            AddedNewWeapon?.Invoke(type);
        }
    }
}

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