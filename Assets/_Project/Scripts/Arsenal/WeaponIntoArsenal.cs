using System;
using UnityEngine;

public class WeaponIntoArsenal : MonoBehaviour
{
    [SerializeField] private WeaponType _type;

    private Weapon _weapon;

    public WeaponType Type => _type;

    private void OnDestroy() => 
        _weapon.StatusChanged -= SetActiveState;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        _weapon.StatusChanged += SetActiveState;
        SetActiveState();
    }

    private void SetActiveState() => 
        gameObject.SetActive(_weapon.IsUnblocked);
}