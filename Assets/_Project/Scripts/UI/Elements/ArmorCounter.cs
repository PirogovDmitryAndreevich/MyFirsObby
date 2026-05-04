using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private Image _reloadView;

    private Shooter _shooter;
    private WeaponBase CurrentWeapon;

    private void OnDestroy() =>
        _shooter.StateChanged -= ShowOrNot;

    private void Start() => 
        ShowOrNot();

    public void Construct(Shooter shooter)
    {
        _shooter = shooter;
        _shooter.StateChanged += ShowOrNot;        
    }

    private void ShowOrNot()
    {
        if (_shooter.IsWeaponSelected)
            WeaponSelected();
        else
            WeaponUnselected();
    }

    private void WeaponSelected()
    {
        Unsubscribe();

        gameObject.SetActive(true);

        CurrentWeapon = _shooter.CurrentWeapon;

        Subscribe();
        SyncState();
    }

    private void WeaponUnselected()
    {
        gameObject.SetActive(false);
        Unsubscribe();
    }

    private void SyncState()
    {
        if (CurrentWeapon.IsReloading)
        {
            ShowReload();
            UpdateReload(CurrentWeapon.ReloadProgress);
        }
        else
        {
            HideReload();
        }

        UpdateCounterText();
    }

    private void ShowReload() => 
        _reloadView.gameObject.SetActive(true);

    private void HideReload() => 
        _reloadView.gameObject.SetActive(false);

    private void UpdateCounterText() => 
        _counterText.text =
        $"{CurrentWeapon.CurrentAmmo.ToString()}/{CurrentWeapon.MagazineSize}";

    private void UpdateReload(float progress)
    {
        _reloadView.fillAmount = progress;

        if (progress >= 1f)
            _reloadView.gameObject.SetActive(false);
    }

    private void Subscribe()
    {
        if (CurrentWeapon == null)
            return;

        CurrentWeapon.AmmunitionChanged += UpdateCounterText;

        CurrentWeapon.ReloadStarted += ShowReload;
        CurrentWeapon.ReloadFinished += HideReload;
        CurrentWeapon.ReloadProgressChanged += UpdateReload;
    }

    private void Unsubscribe()
    {
        if (CurrentWeapon == null)
            return;

        CurrentWeapon.AmmunitionChanged -= UpdateCounterText;

        CurrentWeapon.ReloadStarted -= ShowReload;
        CurrentWeapon.ReloadFinished -= HideReload;
        CurrentWeapon.ReloadProgressChanged -= UpdateReload;
    }
}
