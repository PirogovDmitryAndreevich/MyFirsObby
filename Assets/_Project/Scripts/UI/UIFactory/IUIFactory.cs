public interface IUIFactory : IService
{
    Hud CreateHod(bool isMobile, CharacterHealth characterHealth, Shooter shooter, IWeaponSelector weaponSelector);
    void CreateInventoryButton(WeaponType type, IWeaponSelector weaponSelector);
    RewardAdvPopup CreateRewardAdvPopup();
}