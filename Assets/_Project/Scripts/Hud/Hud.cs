using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] private CashCounter _cashCounter;
    [SerializeField] private ArmorCounter _armorCounter;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private Transform _inventoryView;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private BossTimer _bossTimer;
    [SerializeField] private Button _settings;

    private IUIFactory _uiFactory;
    private IPersistentProgressService _progressService;
    private IWeaponSelector _weaponSelector;
    private IWeaponContainer _weaponContainer;
    private IWindow _pauseWindow;

    public CashCounter LootCounter => _cashCounter;
    public ArmorCounter ArmorCounter => _armorCounter;
    public ScoreCounter ScoreCounter => _scoreCounter;
    public HealthUI HealthUI => _healthUI;
    public Transform InventoryView => _inventoryView;
    public BossTimer BossTimer => _bossTimer;

    private void OnDestroy()
    {
        _progressService.Progress.Weapons.WeaponOpened -= CreateInventoryButton;
        _settings.onClick.RemoveListener(ShowPauseWindow);
    }

    public void Construct( IUIFactory uiFactory, IPersistentProgressService progressService,
        IWeaponSelector weaponSelector, IWeaponContainer container, IWindow pauseWindow)
    {
        _uiFactory = uiFactory;
        _progressService = progressService;
        _weaponSelector = weaponSelector;
        _weaponContainer = container;
        _pauseWindow = pauseWindow;

        foreach (var weapon in _progressService.Progress.Weapons.OpenedWeapons)
            CreateInventoryButton(weapon);

        _progressService.Progress.Weapons.WeaponOpened += CreateInventoryButton;
        _settings.onClick.AddListener(ShowPauseWindow);
    }

    private void ShowPauseWindow() => 
        _pauseWindow.Show();

    private void CreateInventoryButton(WeaponType type) =>
        _uiFactory.CreateInventoryButton(type, _weaponSelector, _weaponContainer.Weapons[type]);
}
