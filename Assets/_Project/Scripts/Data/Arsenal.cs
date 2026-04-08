using System;
using System.Collections.Generic;

[Serializable]
public class Arsenal
{
    public bool IsUnblocked;
    public List<Weapon> Weapons;

    public event Action ArsenalUnblocked;
    public event Action<Weapon> WeaponUnblocked;

    public Arsenal(IStaticDataService staticData)
    {
        IsUnblocked = false;
        Weapons = new();

        foreach (WeaponStaticData weaponStaticData in staticData.AllWeapons())
            Weapons.Add(new Weapon(weaponStaticData));
    }

    public void UnblockArsenal()
    {
        IsUnblocked = true;
        ArsenalUnblocked?.Invoke();
    }

    public void UnblockWeapon(Weapon weapon)
    {
        weapon.Unblock();
        WeaponUnblocked?.Invoke(weapon);
    }
}
