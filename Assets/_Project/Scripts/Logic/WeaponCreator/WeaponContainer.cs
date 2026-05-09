using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : IWeaponContainer
{
    private readonly IGameFactory _gameFactory;
    private Transform _weaponsParent;

    public Dictionary<WeaponType, WeaponBase> Weapons { get; private set; }
    public Transform WeaponsParent => _weaponsParent;

    public WeaponContainer(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
        Weapons = new();
        _weaponsParent = new GameObject("Weapons").transform;
    }

    public void AddNewWeapon(WeaponType type)
    {
        if (Weapons.ContainsKey(type))
            return;

        WeaponBase weapon = _gameFactory.CreateWeapon(type, _weaponsParent);

        Weapons.Add(type, weapon);
        weapon.SetView(false);
    }
}