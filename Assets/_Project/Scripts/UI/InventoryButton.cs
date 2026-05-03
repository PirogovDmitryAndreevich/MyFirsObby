using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _reload;
    [SerializeField] private Image _selected;

    private Button _button;
    private Weapon _weapon;
    private WeaponType _type;
    private IWeaponSelector _weaponSelector;
    private IWeaponCreator _weaponCreator;

    public void Construct(IWeaponSelector weaponSelector, IWeaponCreator weaponCreator ,WeaponType type, Sprite icon)
    {
        _type = type;
        _weaponSelector = weaponSelector;
        _weaponCreator = weaponCreator;
        _icon.sprite = icon;

        _weapon = _weaponCreator.Weapons[_type];
        _button = GetComponent<Button>();
        OnReloadFinished();
        UnselectedView();

        _button.onClick.AddListener(OnPressed);

        _weapon.RuntimeState.ReloadStarted += OnReloadStarted;
        _weapon.RuntimeState.ReloadFinished += OnReloadFinished;
        _weapon.RuntimeState.ReloadProgressChanged += OnReloading;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnPressed);

        _weapon.RuntimeState.ReloadStarted -= OnReloadStarted;
        _weapon.RuntimeState.ReloadFinished -= OnReloadFinished;
        _weapon.RuntimeState.ReloadProgressChanged -= OnReloading;
    }

    private void OnPressed()
    {
        if (_weaponSelector.CurrentWeapon != null && _weaponSelector.CurrentWeapon.Type == _type)
            _weaponSelector.UnselectWeapon();
        else
            _weaponSelector.SelectWeapon(_type);
    }

    public void ShowSelectedView() =>
        _selected.gameObject.SetActive(true);

    public void UnselectedView() =>
        _selected.gameObject.SetActive(false);

    private void OnReloadStarted() =>
        _reload.gameObject.SetActive(true);

    private void OnReloadFinished() =>
        _reload.gameObject.SetActive(false);

    private void OnReloading(float value) =>
        _reload.fillAmount = value;
}