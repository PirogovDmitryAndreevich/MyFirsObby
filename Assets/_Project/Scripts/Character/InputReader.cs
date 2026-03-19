using UnityEngine;

public class InputReader : MonoBehaviour
{
    private bool _isReloadWeapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            _isReloadWeapon = true;
    }

    public bool GetIsReloadWeapon() => GetBoolIsTrigger(ref _isReloadWeapon);

    private bool GetBoolIsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }
}
