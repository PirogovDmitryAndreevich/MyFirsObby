using System;
using System.Collections.Generic;

[Serializable]
public class WeaponsProgress
{
    public List<WeaponType> OpenedWeapons;

    public Action<WeaponType> WeaponOpened;

    public WeaponsProgress()
    {
        OpenedWeapons = new();
    }

    public void OpenWeapon(WeaponType type)
    {
        if (!OpenedWeapons.Contains(type))
        {
            OpenedWeapons.Add(type);
            WeaponOpened(type);
        }
    }
}