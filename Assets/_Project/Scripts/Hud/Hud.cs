using System;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private LootCounter _lootCounter;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private IndicateUI _indicateUI;
    [SerializeField] private Transform _inventoryView;

    private IWeaponSelector _inventory;
    private IUIFactory _uiFactory;
    private IPersistentProgressService _progressService;

    public LootCounter LootCounter => _lootCounter;
    public HealthUI HealthUI => _healthUI;
    public IndicateUI IndicateUI => _indicateUI;
    public Transform InventoryView => _inventoryView;

    private void OnDestroy()
    {
        _progressService.Progress.Arsenal.WeaponUnblocked -= CreateInventoryButton;
    }

    public void Construct(IWeaponSelector inventory, IUIFactory uiFactory, IPersistentProgressService progressService)
    {
        _inventory = inventory;
        _uiFactory = uiFactory;
        _progressService = progressService;

        _progressService.Progress.Arsenal.WeaponUnblocked += CreateInventoryButton;
    }

    private void CreateInventoryButton(Weapon weapon)
    {
        _uiFactory.CreateInventoryButton(_inventory, weapon.Type);
    }
}
