using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Arsenal
{
    public List<WeaponProgress> Weapons;

    public event Action<WeaponProgress> WeaponUnblocked;

    public Arsenal(IStaticDataService staticData)
    {
        Weapons = new();

        foreach (WeaponStaticData weaponStaticData in staticData.AllWeapons())
            Weapons.Add(new WeaponProgress(weaponStaticData));
    }

    public void UnblockWeapon(WeaponProgress weapon)
    {
        weapon.Unblock();
        WeaponUnblocked?.Invoke(weapon);
    }

    public WeaponProgress GetWeaponProgress(WeaponType type) => 
        Weapons.First(w => w.Type == type);
}
