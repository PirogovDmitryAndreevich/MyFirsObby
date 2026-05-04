using System;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private CashCounter _cashCounter;
    [SerializeField] private ArmorCounter _armorCounter;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private Transform _inventoryView;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private BossTimer _bossTimer;

    private IUIFactory _uiFactory;
    private IPersistentProgressService _progressService;
    private IWeaponSelector _weaponSelector;
    private IWeaponContainer _weaponContainer;

    public CashCounter LootCounter => _cashCounter;
    public ArmorCounter ArmorCounter => _armorCounter;
    public ScoreCounter ScoreCounter => _scoreCounter;
    public HealthUI HealthUI => _healthUI;
    public Transform InventoryView => _inventoryView;
    public BossTimer BossTimer => _bossTimer;

    private void OnDestroy() => 
        _progressService.Progress.Weapons.WeaponOpened -= CreateInventoryButton;

    public void Construct( IUIFactory uiFactory, IPersistentProgressService progressService, IWeaponSelector weaponSelector, IWeaponContainer container)
    {
        _uiFactory = uiFactory;
        _progressService = progressService;
        _weaponSelector = weaponSelector;
        _weaponContainer = container;

        foreach (var weapon in _progressService.Progress.Weapons.OpenedWeapons)
            CreateInventoryButton(weapon);

        _progressService.Progress.Weapons.WeaponOpened += CreateInventoryButton;
    }

    private void CreateInventoryButton(WeaponType type) =>
        _uiFactory.CreateInventoryButton(type, _weaponSelector, _weaponContainer.Weapons[type]);
}
