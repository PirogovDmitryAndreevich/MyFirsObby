using System.Collections.Generic;

public interface IWeaponSelector
{
    Weapon CurrentWeapon { get; }
    Dictionary<WeaponType, Weapon> Weapons { get; }
    void SelectWeapon(WeaponType type);
    void UnselectWeapon();
    public void Unsubscribe();
}