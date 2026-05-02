public interface IUIFactory : IService
{
    Hud CreateHod(bool isMobile, IWeaponSelector weaponSelector, CharacterHealth characterHealth, Shooter shooter);
    void CreateInventoryButton(IWeaponSelector weaponSelector, WeaponType type);
    RewardAdvPopup CreateRewardAdvPopup();
    ArsenalShopWindow CreateArsenalShopWindow();
}