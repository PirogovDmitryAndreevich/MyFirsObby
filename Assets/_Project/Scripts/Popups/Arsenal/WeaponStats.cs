using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _unblockButton;
    [SerializeField] private TMP_Text _unblockPriceText;
    [SerializeField] private WeaponStatsButton _damageButton;
    [SerializeField] private WeaponStatsButton _magazineButton;

    private Weapon _weapon;
    private IPersistentProgressService _progressService;
    private WeaponStaticData _weaponStaticData;

    public WeaponType Type { get; private set; }

    public void Construct(Weapon weapon, WeaponStaticData weaponStaticData, IPersistentProgressService progressService)
    {
        _weapon = weapon;
        _progressService = progressService;
        Type = _weapon.Type;
        _weaponStaticData = weaponStaticData;

        _damageButton.Construct(weapon, progressService, weaponStaticData);
        _magazineButton.Construct(weapon, progressService, weaponStaticData);

        _name.text = _weaponStaticData.Name;
        _icon.sprite = _weaponStaticData.Icon;

        if (_weapon.IsUnblocked)
        {
            SetUnblockView();
        }
        else
        {
            _weapon.StatusChanged += SetUnblockView;
            SetBlockView();
        }
    }

    private void OnDestroy() => 
        _weapon.StatusChanged -= SetUnblockView;

    private void OnUnblockButton() => 
       _progressService.Progress.Arsenal.UnblockWeapon(_weapon);

    private void SetBlockView()
    {
        _damageButton.gameObject.SetActive(false);
        _magazineButton.gameObject.SetActive(false);

        _unblockButton.gameObject.SetActive(true);
        _unblockButton.onClick.AddListener(OnUnblockButton);
        _progressService.Progress.PlayerState.ValueChanged += SetInteractButton;

        _unblockPriceText.text = _weaponStaticData.UnblockPrice.ToString();
    }

    private void SetUnblockView()
    {
        _unblockButton.gameObject.SetActive(false);
        _unblockButton.onClick.RemoveListener(OnUnblockButton);
        _progressService.Progress.PlayerState.ValueChanged -= SetInteractButton;

        _damageButton.gameObject.SetActive(true);
        _magazineButton.gameObject.SetActive(true);
    }

    private void SetInteractButton()
    {
        if (_progressService.Progress.PlayerState.Cash >= _weaponStaticData.UnblockPrice)
            Interact();
        else
            NoInteract();
    }

    private void Interact()
    {
        _unblockButton.interactable = true;
    }

    private void NoInteract()
    {
        _unblockButton.interactable = false;
    }
}