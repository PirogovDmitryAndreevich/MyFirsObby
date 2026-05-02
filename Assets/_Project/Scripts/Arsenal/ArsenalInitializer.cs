using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsenalInitializer : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private ArsenalView _arsenalView;

    [Header("Weapons")]
    [SerializeField] private WeaponIntoArsenal[] _weaponsView;

    private Dictionary<WeaponType, WeaponIntoArsenal> _weapons = new();

    public ArsenalView ArsenalView => _arsenalView;

    public void LoadProgress(PlayerProgress progress)
    {
        _weapons = _weaponsView.ToDictionary(x => x.Type, x => x);
        InitWeapons(progress.Arsenal);
    }

    private void InitWeapons(Arsenal arsenalProgress)
    {
        foreach (WeaponProgress weapon in arsenalProgress.Weapons)
        {
            if (_weapons.ContainsKey(weapon.Type))
                InitWeapon(weapon);
        }
    }

    private void InitWeapon(WeaponProgress weapon) =>
        _weapons[weapon.Type].Initialize(weapon);
}
