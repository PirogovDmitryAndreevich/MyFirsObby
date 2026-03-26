using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsenalView : MonoBehaviour, ISavedProgressReader
{
    [Header("SceneSettings")]
    [SerializeField] private TriggerObserver _triggerObserver;

    [Header("Weapons")]
    [SerializeField] private ArsenalViewWeapon[] _weaponsView;

    [SerializeField] private bool isUnblocked;

    private Dictionary<WeaponType, ArsenalViewWeapon> _weapons;

    private Arsenal _arsenal;

    private void Start()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
        HideAllWeaponsView();        
    }

    private void OnDestroy()
    {
        _triggerObserver.TriggerEnter -= TriggerEnter;
        _triggerObserver.TriggerExit -= TriggerExit;
        _arsenal.AddedNewWeapon -= ShowView;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _arsenal = progress.Arsenal;
        _arsenal.AddedNewWeapon += ShowView;

        _weapons = _weaponsView.ToDictionary(x => x.Type, x => x);
        CheckUnblock();
    }

    private void CheckUnblock()
    {
        if (!_arsenal.IsUnblocked)
            return;

        gameObject.SetActive(_arsenal.IsUnblocked);
        
        foreach (var weapon in _arsenal.Weapons)
            ShowView(weapon.Type);
    }

    private void HideAllWeaponsView()
    {
        foreach (var weapon in _weaponsView)
            weapon.gameObject.SetActive(false);
    }

    private void ShowView(WeaponType type)
    {
        if (_weapons.ContainsKey(type))
            _weapons[type].gameObject.SetActive(true);
        else
            Debug.Log("Unknown weapon type");
    }    

    private void TriggerExit(Collider collider)
    {
        Debug.Log("Exit arsenal trigger observer");
    }

    private void TriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CharacterController>())
            Debug.Log("Show open popup");
    }
}
