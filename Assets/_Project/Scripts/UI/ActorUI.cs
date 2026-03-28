using UnityEngine;

public class ActorUI : MonoBehaviour
{
    [SerializeField] private HpBar _hpBar;
    [SerializeField] private IndicateView _indicate;

    private IHealth _health;
    private ArsenalView _arsenalView;


    private void OnDestroy()
    {
        if (_arsenalView != null)
        {
            _arsenalView.ShowShopIndicate -= ShowActionIndicate;
            _arsenalView.HideShopIndicate -= HideActionIndicate;
        }        

        _health.HealthChanged -= UpdateHpBar;
    }

    public void Construct(IHealth health, ArsenalView arsenal = null)
    {
        _health = health;

        if (arsenal != null)
        {
            _arsenalView = arsenal;
            _arsenalView.ShowShopIndicate += ShowActionIndicate;
            _arsenalView.HideShopIndicate += HideActionIndicate;
        }

        _health.HealthChanged += UpdateHpBar;
    }

    private void HideActionIndicate() =>
        _indicate.Hide();

    private void ShowActionIndicate() =>
        _indicate.Show();

    private void UpdateHpBar() =>
        _hpBar.SetValue(_health.Current, _health.Max);
}