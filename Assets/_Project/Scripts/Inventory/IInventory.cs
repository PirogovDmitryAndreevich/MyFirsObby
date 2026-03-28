using System;
using System.Collections.Generic;

public interface IInventory
{
    Dictionary<WeaponType, OpenedWeapon> OpenedWeapons { get; }

    event Action<WeaponType> OpenedNewWeapon;
    event Action<WeaponType> UpdatedWeapon;

    void OpenNewWeapon(WeaponType type);
    void SelectWeapon(WeaponType type);
    void UpdateWeapon(WeaponType type);
}