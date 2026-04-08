using UnityEditor;
using UnityEditor.MPE;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IWeaponUpdaterService _weaponUpdater;
    private readonly IConveerUpdaterService _conveerUpdater;

    private Hud _hud { get; set; }

    public UIFactory(IAssets assets, IPersistentProgressService progressService, IStaticDataService staticData, IWeaponUpdaterService weaponUpdater, IConveerUpdaterService conveerUpdater)
    {
        _assets = assets;
        _progressService = progressService;
        _staticData = staticData;
        _weaponUpdater = weaponUpdater;
        _conveerUpdater = conveerUpdater;
    }

    public Hud CreateHod(bool isMobile, IWeaponSelector weaponSelector, CharacterHealth characterHealth)
    {
        GameObject hud = isMobile
            ? CreateHudMobile()
            : CreateHudDesktop();

        _hud = hud.GetComponent<Hud>();

        _hud.Construct(weaponSelector,this,_progressService);
        _hud.LootCounter.Construct(_progressService.Progress.PlayerState);
        _hud.HealthUI.Construct(characterHealth);

        foreach (Weapon weapon in _progressService.Progress.Arsenal.Weapons)
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

        button.Construct(weaponSelector, type);
    }

    public ArsenalShopWindow CreateArsenalShopWindow()
    {
        ArsenalShopWindow arsenalShop = _assets.Instantiate(AssetsPath.ArsenalShopPath).GetComponent<ArsenalShopWindow>();

        foreach (Weapon weapon in _progressService.Progress.Arsenal.Weapons)
            CreateWeaponsStats(weapon, _staticData.ForWeaponsData(weapon.Type), arsenalShop.WeaponsStatsParent);

        return arsenalShop;
    }

    public ConveerShop CreateConveerShop()
    {
        ConveerShop conveerShop = _assets.Instantiate(AssetsPath.ConveerShopPath).GetComponent<ConveerShop>();

        foreach (ConveerLineProgress line in _progressService.Progress.Conveers.ConveerLines)
            CreateConveerStat(line, conveerShop.ConveerStatsParent);

        return conveerShop;
    }

    private void CreateConveerStat(ConveerLineProgress progressLine, Transform parent)
    {
        ConveerStat conveerStat = _assets.Instantiate(AssetsPath.ConveerStatsPath, parent).GetComponent<ConveerStat>();
        conveerStat.Construct(_progressService, _conveerUpdater, _staticData.ForConveers(progressLine.Numbers), progressLine);
    }

    private void CreateWeaponsStats(Weapon weapon, WeaponStaticData staticData, Transform parent)
    {
        WeaponStats weaponStats = _assets.Instantiate(AssetsPath.WeaponStatsPath, parent).GetComponent<WeaponStats>();
        weaponStats.Construct(weapon, staticData, _progressService, _weaponUpdater);
    }

    private GameObject CreateHudMobile() =>
       _assets.Instantiate(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
       _assets.Instantiate(AssetsPath.HudDesktopPath);
}