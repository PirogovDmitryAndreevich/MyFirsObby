using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryButton : MonoBehaviour
{
    WeaponType _type;
    private Button _button;

    public event Action<WeaponType> ButtonPressed;

    public void Construct(WeaponType type)
    {
        _type = type;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnPressed);
    }

    private void OnDestroy() => 
        _button.onClick.RemoveListener(OnPressed);

    private void OnPressed() => 
        ButtonPressed?.Invoke(_type);
}