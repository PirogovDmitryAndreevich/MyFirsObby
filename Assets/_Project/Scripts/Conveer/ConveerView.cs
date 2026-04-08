using UnityEngine;

public class ConveerView : MonoBehaviour, IInteractObject
{
    [SerializeField] private TriggerObserver _openWindowTrigger;

    private IIndicateUI _indicator;
    private IWindow _conveerShopWindow;

    private void OnDestroy()
    {
        _openWindowTrigger.TriggerEnter -= ShowIndicator;
        _openWindowTrigger.TriggerExit -= HideIndicator;
    }

    public void Construct(IIndicateUI indicator, IWindow conveerShopWindow)
    {
        _indicator = indicator;
        _conveerShopWindow = conveerShopWindow;

        _openWindowTrigger.TriggerEnter += ShowIndicator;
        _openWindowTrigger.TriggerExit += HideIndicator;
        _indicator.Hide();
    }

    public void Interact() =>
        _conveerShopWindow.Show();

    private void ShowIndicator(Collider collider)
    {
        _indicator.Show();

        if (collider.gameObject.TryGetComponent(out IInteractable interactable))
            interactable.SetInteractable(this);
    }

    private void HideIndicator(Collider collider) =>
        _indicator.Hide();
}