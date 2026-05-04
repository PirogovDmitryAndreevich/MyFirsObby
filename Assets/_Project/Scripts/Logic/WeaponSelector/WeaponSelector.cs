using System.Collections.Generic;

public class WeaponSelector : IWeaponSelector
{
    private Character _character;
    private readonly IWeaponContainer _weaponCreator;
    private InventoryButton _current;

    public WeaponBase CurrentWeapon { get; private set; }

    public WeaponSelector(Character character, IWeaponContainer weaponCreator)
    {
        InventoryButtons = new();

        _character = character;
        _weaponCreator = weaponCreator;
    }

    public Dictionary<WeaponType, InventoryButton> InventoryButtons { get; private set; }

    public void SelectWeapon(WeaponType type)
    {
        if (CurrentWeapon != null)
            HideWeapon();

        if (_current != null)
            _current.UnselectedView();

        ShowWeapon(type);

        _current = InventoryButtons[type];
        _current.ShowSelectedView();
        
        _character.Shooter.SelectWeapon(CurrentWeapon);
    }

    public void UnselectWeapon()
    {
        if (CurrentWeapon != null)
            HideWeapon();

        if (_current != null)
            _current.UnselectedView();

        CurrentWeapon = null;
        _current = null;

        _character.Shooter.UnselectWeapon();
    }

    private void ShowWeapon(WeaponType type)
    {
        CurrentWeapon = _weaponCreator.Weapons[type];

        var weaponPoint = _character.CurrentView.WeaponPoint;

        CurrentWeapon.transform.SetParent(weaponPoint);
        CurrentWeapon.transform.position = weaponPoint.position;
        CurrentWeapon.transform.rotation = weaponPoint.rotation;

        CurrentWeapon.SetView(true);
    }

    private void HideWeapon()
    {
        CurrentWeapon.transform.SetParent(_weaponCreator.WeaponsParent);
        CurrentWeapon.SetView(false);
    }
}