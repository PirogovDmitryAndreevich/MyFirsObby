using System.Collections.Generic;
using UnityEngine;

public interface IWeaponContainer : IService
{
    Dictionary<WeaponType, WeaponBase> Weapons { get; }

    Transform WeaponsParent { get; }

    void AddNewWeapon(WeaponType type);
}
