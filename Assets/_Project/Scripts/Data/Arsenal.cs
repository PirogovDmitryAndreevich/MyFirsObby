using System;
using System.Collections.Generic;

[Serializable]
public class Arsenal
{
    public bool IsUnblocked;
    public List<WeaponProgress> Weapons;

    public event Action ArsenalUnblocked;
    public event Action<WeaponProgress> WeaponUnblocked;

    public Arsenal(IStaticDataService staticData)
    {
        IsUnblocked = false;
        Weapons = new();

        foreach (WeaponStaticData weaponStaticData in staticData.AllWeapons())
            Weapons.Add(new WeaponProgress(weaponStaticData));
    }

    public void UnblockArsenal()
    {
        IsUnblocked = true;
        ArsenalUnblocked?.Invoke();
    }

    public void UnblockWeapon(WeaponProgress weapon)
    {
        weapon.Unblock();
        WeaponUnblocked?.Invoke(weapon);
    }
}
