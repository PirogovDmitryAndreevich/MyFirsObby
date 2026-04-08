using UnityEngine;

public class ArsenalOpener : MonoBehaviour
{
    [SerializeField] private ArsenalInitializer _arsenal;
    [SerializeField] private ArsenalOpenerView _view;
    [SerializeField] private TriggerObserver _observer;

    private IPersistentProgressService _progress;

    private void OnDestroy()
    {
        _observer.TriggerEnter -= TriggerEnter;
        _progress.Progress.PlayerState.ValueChanged -= SetView;
    }

    public void Construct(IPersistentProgressService progress)
    {
        _progress = progress;

        _progress.Progress.PlayerState.ValueChanged += SetView;
        _observer.TriggerEnter += TriggerEnter;
    }

    public void SetActiveState(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
            SetView();
    }

    private void TriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Character character))
            UnblockArsenal();
    }

    private void SetView()
    {
        if (_progress.Progress.PlayerState.Cash >= _arsenal.UnblockPrice)
            _view.SetGreenColor();
        else
            _view.SetRedColor();
    }

    private void UnblockArsenal()
    {
        if (_progress.Progress.PlayerState.Cash >= _arsenal.UnblockPrice)
            _progress.Progress.Arsenal.UnblockArsenal();
    }
}
