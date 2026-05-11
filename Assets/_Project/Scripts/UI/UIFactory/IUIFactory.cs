public interface IUIFactory : IService
{
    Hud CreateHod(bool isMobile, CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector, IWeaponContainer weaponContainer);
    void CreateInventoryButton(WeaponType type, IWeaponSelector weaponSelector, WeaponBase weapon);
    RewardAdvPopup CreateRewardAdvPopup();
    AdNotification CreateAdNotification();
}