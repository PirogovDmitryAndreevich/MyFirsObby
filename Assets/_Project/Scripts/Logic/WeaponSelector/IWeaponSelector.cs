using System.Collections.Generic;

public interface IWeaponSelector
{
    public Dictionary<WeaponType, Weapon> OpenedWeapons { get; }

    void SelectWeapon(WeaponType type);
}