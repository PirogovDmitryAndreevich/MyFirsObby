using System.Collections.Generic;
using System.Linq;

public class Inventory : ISavedProgressReader
{
    private PlayerProgress _progress;
    private CharacterController _character;
    private InventoryView _inventoryView;

    private ISaveLoadService _saveLoadService;

    public Dictionary<WeaponType, WeaponStaticData> Weapons { get; private set; }
    public Dictionary<WeaponType, OpenedWeapon> OpenedWeapons { get; private set; }

    public void Construct(CharacterController character, Dictionary<WeaponType, WeaponStaticData> weapons,
        InventoryView inventoryView, ISaveLoadService saveLoadService)
    {
        _character = character;
        Weapons = weapons;
        _inventoryView = inventoryView;
        _saveLoadService = saveLoadService;

        _inventoryView.SelectWeapon += SelectWeapon;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _progress = progress;

        OpenedWeapons = progress.Arsenal.Weapons.ToDictionary(x => x.Type, x => x);

        foreach (var button in OpenedWeapons)
            _inventoryView.AddButton(button.Key);
    }

    public void OpenNewWeapon(WeaponType type)
    {
        if (!OpenedWeapons.ContainsKey(type))
        {
            var weapon = _progress.Arsenal.AddWeapon(type);
            OpenedWeapons.Add(type, weapon);
        }


        _inventoryView.AddButton(type);
    }

    public void UpdateWeapon(WeaponType type)
    {

    }

    private void SelectWeapon(WeaponType type) =>
        _character.SelectWeapon(Weapons[type], OpenedWeapons[type].DamageLevel, OpenedWeapons[type].MagazineLevel);
}