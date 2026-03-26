using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventoryButton _buttonPrefab;

    public event Action<WeaponType> SelectWeapon;

    private List<InventoryButton> _buttons = new List<InventoryButton>();

    private void OnDestroy()
    {
        foreach (var button in _buttons)        
            button.ButtonPressed -= OnSelectWeapon;        
    }

    public void AddButton(WeaponType type)
    {
        InventoryButton newButton = Instantiate(_buttonPrefab, transform);
        newButton.Construct(type);
        newButton.ButtonPressed += OnSelectWeapon;
        _buttons.Add(newButton);
    }

    private void OnSelectWeapon(WeaponType type) => 
        SelectWeapon?.Invoke(type);
}