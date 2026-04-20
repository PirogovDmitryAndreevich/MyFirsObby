public interface IWeaponUpdaterService : IService
{
    void UnblockWeapon(WeaponProgress weapon);
    void UpdateWeapon(WeaponsUpdateType type, WeaponProgress weapon);
}