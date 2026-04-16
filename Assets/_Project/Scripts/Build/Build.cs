using UnityEngine;

public class Build : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private Transform _playerInitialPoint;
    [SerializeField] private Transform _conveerInitialPoint;
    [SerializeField] private Transform _arsenalInitialPoint;
    [SerializeField] private Transform _floorView;
    [SerializeField] private Transform _wallView;
    [SerializeField] private OpenerFloor _openerFloor;
    [SerializeField] private OpenerWall _openerWall;

    private BuildProgress _progress;

    public OpenerFloor FloorOpener => _openerFloor;
    public OpenerWall WallOpener => _openerWall;
    public Transform PlayerInitialPoint => _playerInitialPoint;
    public Transform ConveerInitialPoint => _conveerInitialPoint;
    public Transform ArsenalInitialPoint => _arsenalInitialPoint;

    public void LoadProgress(PlayerProgress progress)
    {
        _progress = progress.BuildProgress;

        _floorView.gameObject.SetActive(_progress.IsFloorOpened);
        FloorOpener.gameObject.SetActive(false);

        _wallView.gameObject.SetActive(_progress.IsWallOpened);
        WallOpener.gameObject.SetActive(false);
    }

    public void ShowFloorOpener() =>
        FloorOpener.gameObject.SetActive(true);

    public void ShowWallOpener() =>
        WallOpener.gameObject.SetActive(true);
}
