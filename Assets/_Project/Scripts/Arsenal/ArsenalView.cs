using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsenalView : MonoBehaviour, IInteractObject
{
    [Header("SceneSettings")]
    [SerializeField] private TriggerObserver _triggerObserver;
    [Header("Weapons")]
    [SerializeField] private ArsenalViewWeapon[] _weaponsView;
    [Header("For Editor")]
    [SerializeField] private bool isUnblocked;

    public event Action ShowArsenalShop;

    private Dictionary<WeaponType, ArsenalViewWeapon> _weapons;

    private IIndicateUI _indicator;
    private IInventory _inventory;
    private IPersistentProgressService _progress;

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

        _inventory.OpenedNewWeapon -= ShowView;
    }

    public void Construct(IPersistentProgressService progress, IInventory inventory, IIndicateUI indicator)
    {
        _inventory = inventory;
        _progress = progress;
        _indicator = indicator;
        _weapons = _weaponsView.ToDictionary(x => x.Type, x => x);

        _inventory.OpenedNewWeapon += ShowView;
        CheckUnblock();
    }

    public void Interact() => 
        ShowArsenalShop?.Invoke();

    public void ShowView(WeaponType type)
    {
        if (_weapons.ContainsKey(type))
            _weapons[type].gameObject.SetActive(true);
        else
            Debug.Log("Unknown weapon type");
    }

    private void CheckUnblock()
    {
        isUnblocked = _progress.Progress.Arsenal.IsUnblocked;

        if (!isUnblocked)
            return;

        gameObject.SetActive(isUnblocked);
        
        foreach (OpenedWeapon weapon in _progress.Progress.Arsenal.Weapons)
            ShowView(weapon.Type);
    }

    private void HideAllWeaponsView()
    {
        foreach (ArsenalViewWeapon weapon in _weaponsView)
            weapon.gameObject.SetActive(false);
    }

    private void TriggerExit(Collider collider) =>
        _indicator.Show();

    private void TriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent( out IInteractable interactable))
        {
            interactable.SetInteractable(this);
            _indicator.Hide();
        }
    }
}
