using System.Collections.Generic;

public class WeaponSelector : IWeaponSelector
{
    private Character _character;
    private readonly IWeaponCreator _weaponCreator;
    private InventoryButton _current;

    public Weapon CurrentWeapon { get; private set; }

    public WeaponSelector(Character character, IWeaponCreator weaponCreator)
    {
        InventoryButtons = new();

        _character = character;
        _weaponCreator = weaponCreator;
    }

    public Dictionary<WeaponType, InventoryButton> InventoryButtons { get; private set; }


    public void SelectWeapon(WeaponType type)
    {
        CurrentWeapon = _weaponCreator.Weapons[type];

        if (_current != null)
            _current.UnselectedView();

        _current = InventoryButtons[type];
        _current.ShowSelectedView();
        
        _character.SelectWeapon(CurrentWeapon);
    }

    public void UnselectWeapon()
    {
        CurrentWeapon = null;

        if (_current != null)
            _current.UnselectedView();

        _current = null;

        _character.UnselectWeapon();
    }
}