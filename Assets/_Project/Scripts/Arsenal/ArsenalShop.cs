using UnityEngine;
using UnityEngine.UI;

public class ArsenalShop : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _addWeapon;

    private Inventory _inventory;
    private ArsenalView _arsenalView;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
        _addWeapon.onClick.AddListener(AddWeapon);
    }

    private void OnDestroy()
    {
        _closeButton?.onClick.RemoveListener(Hide);
        _addWeapon?.onClick.RemoveListener(AddWeapon);
        _arsenalView.ShowArsenalShop -= Show;
    }

    public void Construct(Inventory inventory, ArsenalView arsenalView)
    {
        _inventory = inventory;
        _arsenalView = arsenalView;

        _arsenalView.ShowArsenalShop += Show;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        
    }

    private void AddWeapon()
    {
        _inventory.OpenNewWeapon(WeaponType.M1911);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}