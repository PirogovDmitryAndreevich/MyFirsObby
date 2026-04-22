using UnityEngine;

public class WeaponIntoArsenal : MonoBehaviour
{
    [SerializeField] private WeaponType _type;

    private WeaponProgress _weapon;

    public WeaponType Type => _type;

    private void OnDestroy()
    {
        if (_weapon != null)
            _weapon.StatusChanged -= SetActiveState;
    }

    public void Initialize(WeaponProgress weapon)
    {
        _weapon = weapon;
        Debug.Log($"{_weapon.Type.ToString()} initialized");

        _weapon.StatusChanged += SetActiveState;
        SetActiveState();
    }

    private void SetActiveState() =>
        gameObject.SetActive(_weapon.IsUnblocked);
}