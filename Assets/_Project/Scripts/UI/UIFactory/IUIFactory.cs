public interface IUIFactory : IService
{
    Hud CreateHod(bool isMobile, IWeaponSelector weaponSelector, CharacterHealth characterHealth, Shooter shooter);
    void CreateInventoryButton(IWeaponSelector weaponSelector, WeaponType type);
    ArsenalShopWindow CreateArsenalShopWindow();
    ConveerShop CreateConveerShop();
}