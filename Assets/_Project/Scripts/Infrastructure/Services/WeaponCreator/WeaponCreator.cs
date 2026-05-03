using System.Collections.Generic;

public class WeaponCreator : IWeaponCreator
{
    private readonly IStaticDataService _staticData;
    private readonly IPoolService _poolService;
    private readonly IAssets _assets;
    private readonly ICoroutineRunner _coroutineRunner;

    public Dictionary<WeaponType, Weapon> Weapons { get; private set; }

    public WeaponCreator(IStaticDataService staticData, IPoolService poolService, IAssets assets, ICoroutineRunner coroutineRunner)
    {
        _staticData = staticData;
        _poolService = poolService;
        _assets = assets;
        _coroutineRunner = coroutineRunner;

        Weapons = new();
    }

    public void AddNewWeapon(WeaponType type)
    {
        if (!Weapons.ContainsKey(type))
        {
            WeaponStaticData weaponStaticData = _staticData.ForWeaponsData(type);

            Weapon weapon = new Weapon(weaponStaticData, _poolService, _assets, _coroutineRunner);
            Weapons.Add(type, weapon);
        }
    }
}