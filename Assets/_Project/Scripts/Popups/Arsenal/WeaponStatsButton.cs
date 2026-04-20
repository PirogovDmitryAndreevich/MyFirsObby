using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatsButton : MonoBehaviour
{
    [SerializeField] private WeaponsUpdateType _type;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _price;

    private WeaponStaticData _weaponStaticData;
    private WeaponProgress _weapon;
    private IPersistentProgressService _progressService;
    private IWeaponUpdaterService _updaters;

    private void OnEnable()
    {
        _weapon.WeaponsUpdate += UpdatePrice;
        _progressService.Progress.PlayerState.ValueChanged += UpdateState;
        _button.onClick.AddListener(OnLevelUpdate);

        UpdatePrice(_type);
        UpdateState();
    }

    private void OnDisable()
    {
        _weapon.WeaponsUpdate -= UpdatePrice;
        _progressService.Progress.PlayerState.ValueChanged -= UpdateState;
        _button.onClick.RemoveListener(OnLevelUpdate);
    }

    public void Construct(WeaponProgress weapon, IPersistentProgressService progressService, IWeaponUpdaterService updaters,WeaponStaticData staticData)
    {
        _weaponStaticData = staticData;
        _weapon = weapon;
        _progressService = progressService;
        _updaters = updaters;
    }

    private void UpdateState()
    {
        if (CheckUnblockButton())
            SetUnblockState();
        else
            SetBlockState();
    }

    private void UpdatePrice(WeaponsUpdateType type)
    {
        if (type != _type)
            return;

        PriceWeaponLevels nextLevelStats = _weaponStaticData.GetNextLevelInfo(_weapon.GetLevel(_type), _type);

        if (nextLevelStats != null)        
            _price.text = nextLevelStats.Price.ToString();
        
        else        
            _price.text = "MAX";
    }

    private bool CheckUnblockButton()
    {
        if (!_weapon.CanUpgrade(_type))
            return false;

        int? nextLevelPrice = _weaponStaticData
            .GetNextLevelInfo(_weapon.GetLevel(_type), _type)?.Price;

        return nextLevelPrice.HasValue &&
               nextLevelPrice.Value <= _progressService.Progress.PlayerState.Cash;
    }

    private void OnLevelUpdate() =>
        _updaters.UpdateWeapon(_type, _weapon);

    private void SetBlockState() =>
        _button.interactable = false;

    private void SetUnblockState() =>
        _button.interactable = true;
}