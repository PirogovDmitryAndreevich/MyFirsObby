using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory : IInventory, ISavedProgress
{
    private CharacterController _character;

    public event Action<WeaponType> OpenedNewWeapon;
    public event Action<WeaponType> UpdatedWeapon;

    private ISaveLoadService _saveLoadService;
    private IStaticDataService _staticData;
    private IPersistentProgressService _progress;

    public Dictionary<WeaponType, OpenedWeapon> OpenedWeapons { get; private set; }

    public void Construct(CharacterController character, IStaticDataService staticData, IPersistentProgressService progress, ISaveLoadService saveLoadService)
    {
        _character = character;
        _saveLoadService = saveLoadService;
        _staticData = staticData;
        _progress = progress;

        OpenedWeapons = _progress.Progress.Arsenal.Weapons.ToDictionary(x => x.Type, x => x);
    }

    public void UpdateProgress(PlayerProgress progress)
    {

    }

    public void LoadProgress(PlayerProgress progress)
    {

    }

    public void OpenNewWeapon(WeaponType type)
    {
        if (OpenedWeapons.ContainsKey(type))
            return;

        var weapon = _progress.Progress.Arsenal.AddWeapon(type);
        OpenedWeapons.Add(type, weapon);
        OpenedNewWeapon?.Invoke(type);

        //_saveLoadService.SaveProgress();
    }

    public void UpdateWeapon(WeaponType type)
    {
        //_saveLoadService.SaveProgress();
    }

    public void SelectWeapon(WeaponType type) =>
        _character.SelectWeapon(_staticData.ForWeaponsData(type), OpenedWeapons[type].DamageLevel, OpenedWeapons[type].MagazineLevel);
}