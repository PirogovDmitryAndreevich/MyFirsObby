public class OpenerFloor : Opener
{
    private ISaveLoadService _save;

    private void OnDestroy() => 
        UnsubscribeCash();

    public void Construct(ISaveLoadService save,IPersistentProgressService progress, LevelStaticData staticData)
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
        OpenFloor();
    }

    private void OpenFloor()
    {
        Progress.BuildProgress.OpenFloor();
        _save.SaveProgress();
    }
}
