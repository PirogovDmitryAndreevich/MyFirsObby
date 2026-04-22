using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private CashCounter _cashCounter;
    [SerializeField] private ArmorCounter _armorCounter;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private IndicateUI _indicateUI;
    [SerializeField] private Transform _inventoryView;
    [SerializeField] private ScoreCounter _scoreCounter;

    private IWeaponSelector _weaponSelector;
    private IUIFactory _uiFactory;
    private IPersistentProgressService _progressService;

    public CashCounter LootCounter => _cashCounter;
    public ArmorCounter ArmorCounter => _armorCounter;
    public ScoreCounter ScoreCounter => _scoreCounter;
    public HealthUI HealthUI => _healthUI;
    public IndicateUI IndicateUI => _indicateUI;
    public Transform InventoryView => _inventoryView;

    private void OnDestroy()
    {
        _progressService.Progress.Arsenal.WeaponUnblocked -= CreateInventoryButton;
        _weaponSelector.Unsubscribe();
    }

    public void Construct(IWeaponSelector weaponSelector, IUIFactory uiFactory, IPersistentProgressService progressService)
    {
        _weaponSelector = weaponSelector;
        _uiFactory = uiFactory;
        _progressService = progressService;

        _progressService.Progress.Arsenal.WeaponUnblocked += CreateInventoryButton;
    }

    private void CreateInventoryButton(WeaponProgress weapon) => 
        _uiFactory.CreateInventoryButton(_weaponSelector, weapon.Type);
}
