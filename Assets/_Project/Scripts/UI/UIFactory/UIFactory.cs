using System.Linq;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IWeaponUpdaterService _weaponUpdater;
    private readonly IRewardAdvService _rewardAdvService;

    private Hud _hud { get; set; }

    public UIFactory(IAssets assets, IPersistentProgressService progressService, IStaticDataService staticData,
        IWeaponUpdaterService weaponUpdater, IRewardAdvService rewardAdvService)
    {
        _assets = assets;
        _progressService = progressService;
        _staticData = staticData;
        _weaponUpdater = weaponUpdater;
        _rewardAdvService = rewardAdvService;
    }

    public Hud CreateHod(bool isMobile, IWeaponSelector weaponSelector, CharacterHealth characterHealth, Shooter shooter)
    {
        GameObject hud = isMobile
            ? CreateHudMobile()
            : CreateHudDesktop();

        _hud = hud.GetComponent<Hud>();

        _hud.Construct(weaponSelector,this,_progressService);
        _hud.LootCounter.Construct(_progressService.Progress.PlayerState);
        _hud.HealthUI.Construct(characterHealth);
        _hud.ArmorCounter.Construct(shooter);
        _hud.ScoreCounter.Construct(_progressService.Progress.PlayerState);

        foreach (WeaponProgress weapon in _progressService.Progress.Arsenal.Weapons)
        {
            if (weapon.IsUnblocked)
                CreateInventoryButton(weaponSelector, weapon.Type);
        }

        return _hud;
    }

    public void CreateInventoryButton(IWeaponSelector weaponSelector, WeaponType type)
    {
        InventoryButton button =
                        _assets.Instantiate(AssetsPath.InventoryButtonPath, _hud.InventoryView).GetComponent<InventoryButton>();

        button.Construct(weaponSelector, type, _staticData.ForWeaponsData(type).Icon);

        if (!weaponSelector.InventoryButtons.ContainsKey(type))
            weaponSelector.InventoryButtons[type] = button;
    }

    public RewardAdvPopup CreateRewardAdvPopup()
    {
        RewardAdvPopup popup = _assets.Instantiate(AssetsPath.RewardPopupPath).GetComponent<RewardAdvPopup>();
        popup.Construct(_rewardAdvService);

        return popup;
    }

    public ArsenalShopWindow CreateArsenalShopWindow()
    {
        var obj = _assets.Instantiate(AssetsPath.ArsenalShopPath);
        ArsenalShopWindow arsenalShop = obj.GetComponent<ArsenalShopWindow>();

        foreach (WeaponProgress weapon in _progressService.Progress.Arsenal.Weapons.OrderBy(w => w.Type))
        {
            var staticData = _staticData.ForWeaponsData(weapon.Type);
            CreateWeaponsStats(weapon, staticData, arsenalShop.WeaponsStatsParent);
        }

        return arsenalShop;
    }

    private void CreateWeaponsStats(WeaponProgress weapon, WeaponStaticData staticData, Transform parent)
    {
        WeaponStats weaponStats = _assets.Instantiate(AssetsPath.WeaponStatsPath, parent).GetComponent<WeaponStats>();
        weaponStats.Construct(weapon, staticData, _progressService, _weaponUpdater);
    }   

    private GameObject CreateHudMobile() =>
       _assets.Instantiate(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
       _assets.Instantiate(AssetsPath.HudDesktopPath);
}