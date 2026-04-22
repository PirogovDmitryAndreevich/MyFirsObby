public class OpenerWall : Opener
{
    private ISaveLoadService _save;

    private void OnDestroy() =>
        UnsubscribeCash();

    public void Construct(ISaveLoadService save, IPersistentProgressService progress, LevelStaticData staticData)
    {
        _save = save;

        Progress = progress.Progress;
        Price = staticData.UnblockWallPrice;

        SetState();
        SetText();

        SubscribeCash();
    }

    protected override void OnTrigger()
    {
        base.OnTrigger();
        OpenWall();
    }

    private void OpenWall()
    {
        if (Progress.PlayerState.Cash >= Price)
        {
            Progress.PlayerState.SpendCash(Price);
            Progress.BuildProgress.OpenWall();
            _save.SaveProgress();
        }
    }
}