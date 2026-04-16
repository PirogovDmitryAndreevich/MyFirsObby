using UnityEngine;

public class ConveerOpener : Opener
{
    private ConveerLine _line;
    private IConveerUpdaterService _opener;

    private void OnDestroy()
    {
        UnsubscribeCash();
        _line.LineProgress.ConveerOpened -= SetCurrentViewState;
    }

    public void Construct(ConveerLine line, PlayerProgress progress, IConveerUpdaterService opener)
    {
        _opener = opener;
        _line = line;

        Progress = progress;
        Price = _line.LineStaticInfo.UnblockPrice;

        SetState();
        SetText();

        SubscribeCash();
        _line.LineProgress.ConveerOpened += SetCurrentViewState;

        SetCurrentViewState();
    }

    protected override void OnTrigger()
    {
        base.OnTrigger();
        _opener.UnblockConveer(_line.LineProgress);
    }

    private void SetCurrentViewState() =>
        gameObject.SetActive(!_line.LineProgress.IsOpened);
   
}
