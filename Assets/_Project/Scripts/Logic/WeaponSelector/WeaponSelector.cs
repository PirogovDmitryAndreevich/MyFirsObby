using System.Collections.Generic;

public class WeaponSelector : IWeaponSelector
{
    private Character _character;

    private IPersistentProgressService _progress;
    private readonly IGameFactory _gameFactory;

    public Weapon CurrentWeapon { get; private set; }

    public WeaponSelector(Character character, IGameFactory gameFactory, IPersistentProgressService progress)
    {
        Weapons = new Dictionary<WeaponType, Weapon>();

        _character = character;
        _gameFactory = gameFactory;
        _progress = progress;

        foreach (WeaponProgress weapon in _progress.Progress.Arsenal.Weapons)
        {
            if (weapon.IsUnblocked)
                AddWeapon(weapon.Type, weapon);
        }

        _progress.Progress.Arsenal.WeaponUnblocked += OpenedNewWeapon;
    }

    public void Unsubscribe()
    {
        foreach (Weapon weapon in Weapons.Values)
            weapon.RuntimeState.Unsubscribe();

        _progress.Progress.Arsenal.WeaponUnblocked -= OpenedNewWeapon;
    }

    public Dictionary<WeaponType, Weapon> Weapons { get; private set; }

    public void AddWeapon(WeaponType type, WeaponProgress weapon) =>
        Weapons.Add(type, _gameFactory.CreateWeapon(weapon));

    public void SelectWeapon(WeaponType type)
    {
        CurrentWeapon = Weapons[type];
        _character.SelectWeapon(CurrentWeapon);
    }

    public void UnselectWeapon()
    {
        CurrentWeapon = null;
        _character.UnselectWeapon();
    }

    private void OpenedNewWeapon(WeaponProgress weapon) =>
        AddWeapon(weapon.Type, weapon);
}