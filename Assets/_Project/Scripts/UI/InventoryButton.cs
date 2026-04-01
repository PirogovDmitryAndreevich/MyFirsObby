using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    private Button _button;

    private WeaponType _type;
    private IWeaponSelector _inventory;

    public void Construct(IWeaponSelector inventory, WeaponType type)
    {
        _type = type;
        _inventory = inventory;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnPressed);
    }

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnPressed);

    private void OnPressed() => 
      _inventory.SelectWeapon(_type);
}