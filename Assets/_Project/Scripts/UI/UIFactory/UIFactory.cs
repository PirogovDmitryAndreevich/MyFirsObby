using UnityEditor.MPE;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;

    private Hud _hud { get; set; }

    public UIFactory(IAssets assets, IPersistentProgressService progressService, IStaticDataService staticData)
    {
        _assets = assets;
        _progressService = progressService;
        _staticData = staticData;
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

    private void CreateWeaponsStats(Weapon weapon, WeaponStaticData staticData, Transform parent)
    {
        WeaponStats weaponStats = _assets.Instantiate(AssetsPath.WeaponStatsPath, parent).GetComponent<WeaponStats>();
        weaponStats.Construct(weapon, staticData, _progressService);
    }

    private GameObject CreateHudMobile() =>
       _assets.Instantiate(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
       _assets.Instantiate(AssetsPath.HudDesktopPath);
}