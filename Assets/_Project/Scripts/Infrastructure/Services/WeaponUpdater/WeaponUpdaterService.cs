using UnityEngine;

public class WeaponUpdaterService : IWeaponUpdaterService
{
    private readonly IPersistentProgressService _progress;
    private readonly IStaticDataService _statickData;
    private readonly ISaveLoadService _saveService;

    public WeaponUpdaterService(IPersistentProgressService progress, IStaticDataService staticData, ISaveLoadService saveService)
    {
        _progress = progress;
        _statickData = staticData;
        _saveService = saveService;
    }

    public void UnblockWeapon(WeaponProgress weapon)
    {
        if (weapon.IsUnblocked)
            return;

        int price = _statickData.ForWeaponsData(weapon.Type).UnblockPrice;

        if (_progress.Progress.PlayerState.Cash >= price)
        {
            _progress.Progress.Arsenal.UnblockWeapon(weapon);
            _progress.Progress.PlayerState.SpendCash(price);
            _saveService.SaveProgress();
        }
    }

    public void UpdateWeapon(WeaponsUpdateType type, WeaponProgress weapon)
    {
        if (!weapon.CanUpgrade(type))
            return;

        int price = _statickData.ForWeaponsData(weapon.Type).GetLevelInfo(weapon.GetLevel(type), type).Price;

        if (_progress.Progress.PlayerState.Cash >= price)
        {            
            weapon.UpdateLevel(type);
            _progress.Progress.PlayerState.SpendCash(price);
            _saveService.SaveProgress();
        }
    }
}