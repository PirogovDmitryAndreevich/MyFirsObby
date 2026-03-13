using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene setting")]
    [SerializeField] private CharacterController _character;

    [Header("UI elements")]
    [SerializeField] private TMP_Text _bulletsCountsText;

    private Guns _currentGuns;

    private void OnEnable()
    {
        _currentGuns = _character.SelectedGuns;
        _currentGuns.AmmunitionChanged += UpdateBulletsCounts;   
        UpdateBulletsCounts();
    }

    private void OnDisable()
    {
        _currentGuns.AmmunitionChanged -= UpdateBulletsCounts;
    }

    private void UpdateBulletsCounts()
    {
        _bulletsCountsText.text = $"{_currentGuns.CurrentAmmunition} / {_currentGuns.CurrentMagazineSize}";
    }
}
