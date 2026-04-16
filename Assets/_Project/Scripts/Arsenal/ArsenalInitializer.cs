using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsenalInitializer : MonoBehaviour, ISavedProgressReader
{
    [Header("Arsenal")]
    [SerializeField] private ArsenalView _arsenalView;
    [Header("Opener")]
    [SerializeField] private ArsenalOpener _arsenalOpener;
    [Header("Weapons")]
    [SerializeField] private WeaponIntoArsenal[] _weaponsView;

    private Arsenal _arsenalProgress;
    private Dictionary<WeaponType, WeaponIntoArsenal> _weapons = new();

    public bool IsOpened { get; private set; }
    public ArsenalView ArsenalView => _arsenalView;
    public ArsenalOpener Opener => _arsenalOpener;

    private void OnDestroy() => 
        _arsenalProgress.ArsenalUnblocked -= SetActiveState;

    public void LoadProgress(PlayerProgress progress)
    {
        _arsenalProgress = progress.Arsenal;
        _weapons = _weaponsView.ToDictionary(x => x.Type, x => x);

        InitWeapons(_arsenalProgress);

        ArsenalView.SetActiveView(_arsenalProgress.IsUnblocked);
        Opener.gameObject.SetActive(!_arsenalProgress.IsUnblocked);
        _arsenalProgress.ArsenalUnblocked += SetActiveState;

        SetActiveState();
    }

    private void SetActiveState()
    {
        IsOpened = _arsenalProgress.IsUnblocked;
        _arsenalView.gameObject.SetActive(IsOpened);
        Opener.gameObject.SetActive(!_arsenalProgress.IsUnblocked);
    }

    private void InitWeapons(Arsenal arsenalProgress)
    {
        foreach (Weapon weapon in arsenalProgress.Weapons)
        {
            if (_weapons.ContainsKey(weapon.Type))
                InitWeapon(weapon);
        }
    }

    private void InitWeapon(Weapon weapon) => 
        _weapons[weapon.Type].Initialize(weapon);
}
