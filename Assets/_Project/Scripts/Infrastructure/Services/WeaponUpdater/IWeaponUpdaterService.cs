public interface IWeaponUpdaterService : IService
{
    void UnblockWeapon(Weapon weapon);
    void UpdateWeapon(WeaponsUpdateType type, Weapon weapon);
}