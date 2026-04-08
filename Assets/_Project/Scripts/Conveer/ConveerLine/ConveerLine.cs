using UnityEngine;

public class ConveerLine : MonoBehaviour
{
    [SerializeField] private ConveerNumbers _numbers;
    [SerializeField] private ConveerOpener _opener;
    [SerializeField] private ConveerLineView _view;
    [SerializeField] private SpawnerResources _spawnerResources;

    private ConveerLineProgress _lineProgress;
    private ConveerStaticInfo _lineStaticInfo;

    public ConveerNumbers Numbers => _numbers;
    public bool IsOpened { get; private set; }
    public ConveerLineProgress LineProgress => _lineProgress;
    public ConveerStaticInfo LineStaticInfo => _lineStaticInfo;
    public ConveerOpener Opener => _opener;
    public ConveerLineView ConveerView => _view;
    public SpawnerResources SpawnerResources => _spawnerResources;

    private void OnDestroy() => 
        _lineProgress.ConveerOpened -= OnOpened;

    public void Construct(ConveerLineProgress lineProgress, ConveerStaticInfo staticData, PlayerProgress playerProgress, IConveerUpdaterService updater)
    {
        _lineProgress = lineProgress;
        _lineStaticInfo = staticData;

        IsOpened = _lineProgress.IsOpened;
        _lineProgress.ConveerOpened += OnOpened;

        ConveerView.Construct(this);
        Opener.Construct(this, playerProgress, updater);
        SpawnerResources.Construct(this);
    }

    private void OnOpened() => 
        IsOpened = _lineProgress.IsOpened;
}