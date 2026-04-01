using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HpBar _hpBar;

    private IHealth _health;

    private void OnDestroy()
    {
        _health.HealthChanged -= UpdateHpBar;
    }

    public void Construct(IHealth health)
    {
        _health = health;

        _health.HealthChanged += UpdateHpBar;
    }

    private void UpdateHpBar() =>
        _hpBar.SetValue(_health.Current, _health.Max);
}