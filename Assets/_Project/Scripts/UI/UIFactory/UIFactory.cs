using UnityEngine;

public class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IRewardAdvService _rewardAdvService;

    private Hud _hud { get; set; }

    public UIFactory(IAssets assets, IPersistentProgressService progressService, IStaticDataService staticData,
        IRewardAdvService rewardAdvService)
    {
        _assets = assets;
        _progressService = progressService;
        _staticData = staticData;
        _rewardAdvService = rewardAdvService;
    }

    public Hud CreateHod(bool isMobile, CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector, IWeaponContainer weaponContainer)
    {
        GameObject hud = isMobile
            ? CreateHudMobile()
            : CreateHudDesktop();

        _hud = hud.GetComponent<Hud>();

        _hud.Construct(this, _progressService, weaponSelector, weaponContainer);
        _hud.LootCounter.Construct(_progressService.Progress.PlayerState);
        _hud.HealthUI.Construct(characterHealth);
        _hud.ArmorCounter.Construct(shooter);
        _hud.ScoreCounter.Construct(_progressService.Progress.PlayerState);

        return _hud;
    }

    public void CreateInventoryButton(WeaponType type, IWeaponSelector weaponSelector, WeaponBase weapon)
    {
        InventoryButton button =
                        _assets.Instantiate(AssetsPath.InventoryButtonPath, _hud.InventoryView).GetComponent<InventoryButton>();

        button.Construct(weaponSelector, weapon, type, _staticData.ForWeaponsData(type).Icon);

        if (!weaponSelector.InventoryButtons.ContainsKey(type))
            weaponSelector.InventoryButtons[type] = button;
    }

    public RewardAdvPopup CreateRewardAdvPopup()
    {
        RewardAdvPopup popup = _assets.Instantiate(AssetsPath.RewardPopupPath).GetComponent<RewardAdvPopup>();
        popup.Construct(_rewardAdvService);

        return popup;
    }

    private GameObject CreateHudMobile() =>
       _assets.Instantiate(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
       _assets.Instantiate(AssetsPath.HudDesktopPath);
}