using UnityEngine;
using UnityEngine.UI;

public class ArsenalShop : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _addWeapon;

    private IInventory _inventory;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
        _addWeapon.onClick.AddListener(AddWeapon);
    }

    private void OnDestroy()
    {
        _closeButton?.onClick.RemoveListener(Hide);
        _addWeapon?.onClick.RemoveListener(AddWeapon);
    }

    public void Construct(IInventory inventory)
    {
        _inventory = inventory;
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