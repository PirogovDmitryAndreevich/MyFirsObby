using UnityEngine;

public interface IUIFactory : IService
{
    Hud CreateHod(bool isMobile, IWeaponSelector weaponSelector, CharacterHealth characterHealth);
    void CreateInventoryButton(IWeaponSelector weaponSelector, WeaponType type);
    ArsenalShopWindow CreateArsenalShopWindow();
    ConveerShop CreateConveerShop();
}