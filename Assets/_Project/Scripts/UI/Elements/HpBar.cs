using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;

    public void SetValue(float current, float max) =>
        _hpBar.value = current / max;
}
