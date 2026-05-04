using TMPro;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class WeaponUnblocker : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private WeaponType _type;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;

    private PlayerProgress _progress;
    private MaterialPropertyBlock _block;
    private IWeaponContainer _weaponCreator;
    private ISaveLoadService _saveLoadService;
    private int _price;

    private void Awake() =>
        _block = new MaterialPropertyBlock();

    private void OnDestroy() =>
        _progress.PlayerState.CashChanged -= SetState;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character))
            return;

        if (_progress.PlayerState.Cash >= _price)
        {
            _progress.PlayerState.SpendCash(_price);
            OpenWeapon();
            _saveLoadService.SaveProgress();
        }
    }

    public void Construct(IStaticDataService staticData, IWeaponContainer weaponCreator, ISaveLoadService saveLoadService)
    {
        var weaponStaticData = staticData.ForWeaponsData(_type);

        _weaponCreator = weaponCreator;
        _saveLoadService = saveLoadService;

        _price = weaponStaticData.UnblockPrice;
        _text.text = _price.ToString() + "$";
        _collider.isTrigger = true;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _progress = progress;

        _progress.PlayerState.CashChanged += SetState;
        SetState();

        if (_progress.Weapons.OpenedWeapons.Contains(_type))        
            gameObject.SetActive(false);        
        else        
            gameObject.SetActive(true);
    }

    private void OpenWeapon()
    {
        _weaponCreator.AddNewWeapon(_type);
        _progress.Weapons.OpenWeapon(_type);
        gameObject.SetActive(false);
    }

    private void SetState() =>
        SetInteract(_progress.PlayerState.Cash >= _price);

    private void SetInteract(bool isActive)
    {
        _collider.enabled = isActive;
        SetView(isActive);
    }

    private void SetView(bool isGreen)
    {
        if (isGreen)
            SetGreenColor();
        else
            SetRedColor();
    }

    private void SetRedColor() =>
        SetColor(Color.red);

    private void SetGreenColor() =>
        SetColor(Color.green);

    private void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_block);
    }
}