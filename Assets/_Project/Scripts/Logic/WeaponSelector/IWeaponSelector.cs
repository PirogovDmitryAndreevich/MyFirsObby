using System.Collections.Generic;

public interface IWeaponSelector
{
    Weapon CurrentWeapon { get; }
    Dictionary<WeaponType, InventoryButton> InventoryButtons { get; }
    void SelectWeapon(WeaponType type);
    void UnselectWeapon();
}