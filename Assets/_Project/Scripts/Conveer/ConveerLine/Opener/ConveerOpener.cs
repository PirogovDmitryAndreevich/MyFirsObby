using UnityEngine;

public class ConveerOpener : MonoBehaviour
{
    [SerializeField] private ConveerOpenerView _view;    

    private ConveerLine _line;
    private PlayerProgress _progress;
    private IConveerUpdaterService _opener;

    private void OnDestroy()
    {
        _line.LineProgress.ConveerOpened -= SetCurrentViewState;
        _view.TriggerObserver.TriggerEnter -= TriggerEnter;
    }

    public void Construct(ConveerLine line, PlayerProgress progress, IConveerUpdaterService opener)
    {
        _progress = progress;
        _opener = opener;
        _line = line;

        _view.Construct(line, progress);

        _view.TriggerObserver.TriggerEnter += TriggerEnter;
        _line.LineProgress.ConveerOpened += SetCurrentViewState;

        _view.gameObject.SetActive(!line.LineProgress.IsOpened);
    }

    public void SetActiveState(bool isActive) => 
        _view.gameObject.SetActive(isActive);

    private void SetCurrentViewState() =>
        _view.gameObject.SetActive(!_line.LineProgress.IsOpened);

    private void TriggerEnter(Collider collider)
    {
        if (_progress.PlayerState.Cash < _line.LineStaticInfo.UnblockPrice)
            return;

        if (collider.TryGetComponent(out Character character))
            _opener.UnblockConveer(_line.LineProgress);
    }    
}
