using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : IWeaponContainer
{
    private readonly IStaticDataService _staticData;
    private readonly IBulletsPool _poolService;

    private Transform _weaponsParent;

    public Dictionary<WeaponType, WeaponBase> Weapons { get; private set; }
    public Transform WeaponsParent => _weaponsParent;

    public WeaponContainer(IStaticDataService staticData, IBulletsPool poolService)
    {
        _staticData = staticData;
        _poolService = poolService;

        Weapons = new();

        _weaponsParent = new GameObject("Weapons").transform;
    }

    public void AddNewWeapon(WeaponType type)
    {
        if (Weapons.ContainsKey(type))
            return;

        WeaponStaticData weaponStaticData = _staticData.ForWeaponsData(type);

        WeaponBase weapon = Object.Instantiate(weaponStaticData.Weapon, _weaponsParent);
        weapon.Construct(weaponStaticData, _poolService);

        Weapons.Add(type, weapon);

        weapon.SetView(false);
    }
}