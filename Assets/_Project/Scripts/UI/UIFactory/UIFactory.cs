using UnityEngine;

public class UIFactory : IUIFactory
{
    private readonly IAssets _assets;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IRewardAdvService _rewardAdvService;
    private readonly IPauseService _pauseService;
    private readonly ISaveLoadService _save;

    private Hud _hud { get; set; }

    public UIFactory(IAssets assets, IPersistentProgressService progressService, IStaticDataService staticData,
        IRewardAdvService rewardAdvService, IPauseService pauseService, ISaveLoadService saveLoadService)
    {
        _assets = assets;
        _progressService = progressService;
        _staticData = staticData;
        _rewardAdvService = rewardAdvService;
        _pauseService = pauseService;
        _save = saveLoadService;
    }

    public Hud CreateHod(bool isMobile, CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector, IWeaponContainer weaponContainer)
    {
        GameObject hud = isMobile
            ? CreateHudMobile()
            : CreateHudDesktop();

        _hud = hud.GetComponent<Hud>();

        var pauseWindow = CreatePauseWindow();

        _hud.Construct(this, _progressService, weaponSelector, weaponContainer, pauseWindow);
        _hud.LootCounter.Construct(_progressService.Progress.PlayerState);
        _hud.HealthUI.Construct(characterHealth);
        _hud.ArmorCounter.Construct(shooter);
        _hud.ScoreCounter.Construct(_progressService.Progress.PlayerState);
        _hud.AudioHandler.Construct(_progressService.Progress);

        return _hud;
    }

    public void CreateInventoryButton(WeaponType type, UIAudioHandler audioHandler, IWeaponSelector weaponSelector, WeaponBase weapon)
    {
        InventoryButton button =
                        _assets.Instantiate(AssetsPath.InventoryButtonPath, _hud.InventoryView).GetComponent<InventoryButton>();

        button.Construct(weaponSelector, weapon, audioHandler, type, _staticData.ForWeaponsData(type).Icon);

        if (!weaponSelector.InventoryButtons.ContainsKey(type))
            weaponSelector.InventoryButtons[type] = button;
    }

    public RewardAdvPopup CreateRewardAdvPopup()
    {
        RewardAdvPopup popup = _assets.Instantiate(AssetsPath.RewardPopupPath).GetComponent<RewardAdvPopup>();
        popup.Construct(_rewardAdvService, _pauseService);

        return popup;
    }

    public AdNotification CreateAdNotification()
    {
        AdNotification notification = _assets.Instantiate(AssetsPath.AdNotificationPath)
            .GetComponent<AdNotification>();

        notification.Construct(_pauseService);

        return notification;
    }

    private GameObject CreateHudMobile() =>
       _assets.Instantiate(AssetsPath.HudMobilePath);

    private GameObject CreateHudDesktop() =>
       _assets.Instantiate(AssetsPath.HudDesktopPath);

    private PauseWindow CreatePauseWindow()
    {
        PauseWindow window = _assets.Instantiate(AssetsPath.PauseWindowPath)
            .GetComponent<PauseWindow>();

        window.Construct(_pauseService, _hud.AudioHandler);
        window.AudioSettings.Construct(_progressService.Progress.Audio, _hud.AudioHandler, _save);

        return window;
    }
}