using UnityEngine;

public class ConveerLineView : MonoBehaviour
{
    [SerializeField] ConveerCubeMover _cubeMover;
    [SerializeField] Transform _view;

    private ConveerLine _line;

    public Transform ResourceSpawnPoint => _cubeMover.ResourceSpawnPoint;

    private void OnDestroy() => 
        _line.LineProgress.ConveerOpened -= SetCurrentViewState;

    public void Construct(ConveerLine line)
    {
        _line = line;

        _cubeMover.Construct(line);

        _line.LineProgress.ConveerOpened += SetCurrentViewState;
        SetCurrentViewState();
    }

    public void SetViewState(bool isActive) => 
        _view.gameObject.SetActive(isActive);

    private void SetCurrentViewState() => 
        _view.gameObject.SetActive(_line.LineProgress.IsOpened);
}