using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArsenalView : MonoBehaviour, IInteractObject
{
    [Header("SceneSettings")]
    [SerializeField] private TriggerObserver _triggerObserver;
    [Header("Weapons")]
    [SerializeField] private WeaponView[] _weaponsView;
    [Header("For Editor")]
    [SerializeField] private bool isUnblocked;

    private Dictionary<WeaponType, WeaponView> _weapons;

    private IIndicateUI _indicator;
    private IWindow _arsenalWindow;
    private IPersistentProgressService _progress;

    private void Start()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
        HideAllWeaponsView();
        _indicator.Hide();
    }

    private void OnDestroy()
    {
        _triggerObserver.TriggerEnter -= TriggerEnter;
        _triggerObserver.TriggerExit -= TriggerExit;
    }

    public void Construct(IPersistentProgressService progress, IIndicateUI indicator, IWindow arsenalWindow)
    {
        _progress = progress;
        _indicator = indicator;
        _arsenalWindow = arsenalWindow;
        _weapons = _weaponsView.ToDictionary(x => x.Type, x => x);

        if (_progress.Progress.Arsenal.IsUnblocked)
            return;

        UnblockArsenal();

        foreach (Weapon weapon in _progress.Progress.Arsenal.Weapons)
            ShowView(weapon);
    }

    public void Interact() =>
        _arsenalWindow.Show();

    public void ShowView(Weapon weapon)
    {
        if (_weapons.ContainsKey(weapon.Type))
            _weapons[weapon.Type].gameObject.SetActive(true);
        else
            Debug.Log("Unknown weapon type");
    }

    private void UnblockArsenal()
    {
        isUnblocked = _progress.Progress.Arsenal.IsUnblocked;

        if (!isUnblocked)
            return;

        gameObject.SetActive(isUnblocked);        
    }

    private void HideAllWeaponsView()
    {
        foreach (WeaponView weapon in _weaponsView)
            weapon.gameObject.SetActive(false);
    }

    private void TriggerExit(Collider collider) =>
        _indicator.Hide();

    private void TriggerEnter(Collider collider)
    {
        _indicator.Show();

        if (collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetInteractable(this);
        }
    }
}
