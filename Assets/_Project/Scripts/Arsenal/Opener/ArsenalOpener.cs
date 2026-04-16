public class ArsenalOpener : Opener
{
    private ISaveLoadService _save;

    private void OnDestroy() =>
        UnsubscribeCash();

    public void Construct(ISaveLoadService save, IPersistentProgressService progress, LevelStaticData staticData)
    {
        _save = save;

        Progress = progress.Progress;
        Price = staticData.UnblockArsenalPrice;

        SetState();
        SetText();

        SubscribeCash();
    }

    protected override void OnTrigger()
    {
        base.OnTrigger();
        Progress.Arsenal.UnblockArsenal();
        _save.SaveProgress();
    }  
}