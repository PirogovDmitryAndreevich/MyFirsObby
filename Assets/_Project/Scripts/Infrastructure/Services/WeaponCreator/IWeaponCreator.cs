using System.Collections.Generic;

public interface IWeaponCreator : IService
{
    Dictionary<WeaponType, Weapon> Weapons { get; }

    void AddNewWeapon(WeaponType type);
}
