using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _reload;
    [SerializeField] private Image _selected;
    [SerializeField] private AudioClip _selectSound;
    [SerializeField] private AudioClip _unselectSound;

    private Button _button;
    private WeaponBase _weapon;
    private UIAudioHandler _audio;
    private WeaponType _type;
    private IWeaponSelector _weaponSelector;

    public void Construct(IWeaponSelector weaponSelector, WeaponBase weapon, UIAudioHandler audioHandler, WeaponType type, Sprite icon)
    {
        _type = type;
        _weaponSelector = weaponSelector;
        _icon.sprite = icon;
        _weapon = weapon;
        _audio = audioHandler;

        _button = GetComponent<Button>();
        OnReloadFinished();
        UnselectedView();

        _button.onClick.AddListener(OnPressed);

        _weapon.ReloadStarted += OnReloadStarted;
        _weapon.ReloadFinished += OnReloadFinished;
        _weapon.ReloadProgressChanged += OnReloading;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnPressed);

        _weapon.ReloadStarted -= OnReloadStarted;
        _weapon.ReloadFinished -= OnReloadFinished;
        _weapon.ReloadProgressChanged -= OnReloading;
    }

    private void OnPressed()
    {
        if (_weaponSelector.CurrentWeapon != null && _weaponSelector.CurrentWeapon.Type == _type)
        {
            _audio.PlaySound(_unselectSound);
            _weaponSelector.UnselectWeapon();
        }
        else
        {
            _audio.PlaySound(_selectSound);
            _weaponSelector.SelectWeapon(_type);
        }
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