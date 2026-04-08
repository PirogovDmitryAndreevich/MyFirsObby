using UnityEngine;

public class ArsenalView : MonoBehaviour, IInteractObject
{
    [SerializeField] private TriggerObserver _triggerObserver;

    private IIndicateUI _indicator;
    private IWindow _arsenalWindow;

    private void OnDestroy()
    {
        _triggerObserver.TriggerEnter -= TriggerEnter;
        _triggerObserver.TriggerExit -= TriggerExit;
    }

    public void Construct(IIndicateUI indicator, IWindow arsenalWindow)
    {
        _indicator = indicator;
        _arsenalWindow = arsenalWindow;

        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
        _indicator.Hide();
    }

    public void SetActiveView(bool isActive) => 
        gameObject.SetActive(isActive);

    public void Interact() =>
        _arsenalWindow.Show();

    private void TriggerExit(Collider collider) =>
        _indicator.Hide();

    private void TriggerEnter(Collider collider)
    {
        _indicator.Show();

        if (collider.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetInteractable(this);
        }
    }
}