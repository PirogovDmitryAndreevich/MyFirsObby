using System.Collections.Generic;

public class WeaponSelector : IWeaponSelector
{
    private Character _character;

    private IStaticDataService _staticData;
    private IPersistentProgressService _progress;

    public Dictionary<WeaponType, Weapon> OpenedWeapons { get; private set; }

    public void Construct(Character character, IStaticDataService staticData, IPersistentProgressService progress)
    {
        _character = character;
        _staticData = staticData;
        _progress = progress;

        OpenedWeapons = new Dictionary<WeaponType, Weapon>();

        foreach (Weapon weapon in _progress.Progress.Arsenal.Weapons)
        {
            if (weapon.IsUnblocked && !OpenedWeapons.ContainsKey(weapon.Type))
                OpenedWeapons.Add(weapon.Type, weapon);
        }

        _progress.Progress.Arsenal.WeaponUnblocked += OpenedNewWeapon;
    }

    public void SelectWeapon(WeaponType type) =>
        _character.SelectWeapon(_staticData.ForWeaponsData(type), OpenedWeapons[type].DamageLevel, OpenedWeapons[type].MagazineLevel);


    private void OpenedNewWeapon(Weapon weapon)
    {
        if (!OpenedWeapons.ContainsKey(weapon.Type))
            OpenedWeapons.Add(weapon.Type, weapon);
    }
}