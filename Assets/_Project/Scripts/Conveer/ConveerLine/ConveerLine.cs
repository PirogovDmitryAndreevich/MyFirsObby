using UnityEngine;

public class ConveerLine : MonoBehaviour
{
    [SerializeField] private ConveerNumbers _numbers;
    [SerializeField] private ConveerOpener _opener;
    [SerializeField] private ConveerLineView _view;
    [SerializeField] private SpawnerResources _spawnerResources;

    public ConveerNumbers Numbers => _numbers;
    public bool IsOpened { get; private set; }
    public ConveerLineProgress LineProgress { get; private set; }
    public ConveerStaticInfo LineStaticInfo { get; private set; }
    public ConveerOpener Opener => _opener;
    public ConveerLineView ConveerView => _view;
    public SpawnerResources SpawnerResources => _spawnerResources;

    private void OnDestroy() =>
        LineProgress.ConveerOpened -= OnOpened;

    public void Construct(ConveerLineProgress lineProgress, ConveerStaticInfo staticData, PlayerProgress playerProgress, IConveerUpdaterService updater, IPoolService poolService)
    {
        LineProgress = lineProgress;
        LineStaticInfo = staticData;

        IsOpened = LineProgress.IsOpened;
        LineProgress.ConveerOpened += OnOpened;

        ConveerView.Construct(this);
        Opener.Construct(this, playerProgress, updater);
        SpawnerResources.Construct(this, poolService);
    }

    private void OnOpened() =>
        IsOpened = LineProgress.IsOpened;
}