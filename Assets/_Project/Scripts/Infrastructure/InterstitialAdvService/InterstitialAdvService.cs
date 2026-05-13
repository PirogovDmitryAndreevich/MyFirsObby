using YG;

public class InterstitialAdvService : IInterstitialAdvService
{
    private readonly IUIFactory _uiFactory;
    private AdNotification _notification;

    private bool _isShowing;

    public InterstitialAdvService(IUIFactory gameFactory)
    {
        _uiFactory = gameFactory;

        YG2.onCloseInterAdv += OnCloseInterAdv;
    }

    public bool IsInterstitialReady()
    {
        return !_isShowing
               && !YG2.nowInterAdv
               && YG2.isTimerAdvCompleted;
    }

    public void ShowAdv()
    {
        if (_isShowing)
            return;

        _isShowing = true;

        if (_notification == null)
            _notification = _uiFactory.CreateAdNotification();

        _notification.Show(() =>
        {
            YG2.InterstitialAdvShow();
        });
    }

    private void OnCloseInterAdv()
    {
        _isShowing = false;
    }
}
