using UnityEngine;

public class Build : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private Transform _playerInitialPoint;
    [SerializeField] private Transform _conveerInitialPoint;
    [SerializeField] private Transform _arsenalInitialPoint;
    [SerializeField] private Transform _ATMInitialPoint;
    [SerializeField] private Transform _floorView;
    [SerializeField] private Transform _wallView;
    [SerializeField] private OpenerFloor _openerFloor;
    [SerializeField] private OpenerWall _openerWall;
    [SerializeField] private MoneyBugRewardAdv _moneyBugRewardAdv;

    private BuildProgress _progress;

    public bool IsWallOpened { get; private set; }
    public bool IsFloorOpened { get; private set; }
    public OpenerFloor FloorOpener => _openerFloor;
    public OpenerWall WallOpener => _openerWall;
    public Transform PlayerInitialPoint => _playerInitialPoint;
    public Transform ConveerInitialPoint => _conveerInitialPoint;
    public Transform ArsenalInitialPoint => _arsenalInitialPoint;
    public Transform ATMInitialPoint => _ATMInitialPoint;
    public MoneyBugRewardAdv MoneyBugAdv => _moneyBugRewardAdv;

    private void OnDestroy()
    {
        _progress.WallOpened -= OnWallOpened;
        _progress.FloorOpened -= OnFloorOpened;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _progress = progress.BuildProgress;

        IsWallOpened = _progress.IsFloorOpened;
        IsFloorOpened = _progress.IsWallOpened;

        _floorView.gameObject.SetActive(IsFloorOpened);
        FloorOpener.gameObject.SetActive(false);

        _wallView.gameObject.SetActive(IsWallOpened);
        WallOpener.gameObject.SetActive(false);

        _progress.WallOpened += OnWallOpened;
        _progress.FloorOpened += OnFloorOpened;
    }

    public void ShowFloorOpener() =>
        FloorOpener.gameObject.SetActive(true);

    public void ShowWallOpener() =>
        WallOpener.gameObject.SetActive(true);

    private void OnWallOpened()
    {
        IsWallOpened = _progress.IsWallOpened;
        _wallView.gameObject.SetActive(IsWallOpened);
        WallOpener.gameObject.SetActive(!IsWallOpened);
    }

    private void OnFloorOpened()
    {
        IsFloorOpened = _progress.IsFloorOpened;
        _floorView.gameObject.SetActive(IsFloorOpened);
        FloorOpener.gameObject.SetActive(!IsFloorOpened);
    }
}
