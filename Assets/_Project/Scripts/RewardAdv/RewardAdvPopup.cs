using UnityEngine;
using UnityEngine.UI;

public class RewardAdvPopup : MonoBehaviour
{
    [SerializeField] private Button _see;
    [SerializeField] private Button _cancel;

    private RewardType _currentID;
    private IPauseService _pause;
    private IRewardAdvService _rewardService;

    private void Start() => 
        Hide();

    private void OnEnable()
    {
        _see.onClick.AddListener(SeeAdv);
        _cancel.onClick.AddListener(Cancel);
    }

    private void OnDisable()
    {
        _see.onClick.RemoveListener(SeeAdv);
        _cancel.onClick.RemoveListener(Cancel);
    }

    public void Construct(IRewardAdvService rewardService, IPauseService pause)
    {
        _pause = pause;
        _rewardService = rewardService;
    }

    public void Show(RewardType typeID)
    {
        _currentID = typeID;
        gameObject.SetActive(true);
        _pause.Pause();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _pause.Run();
    }

    private void SeeAdv()
    {
        _rewardService.ShowRewardAdv(_currentID);
        Hide();
    }

    private void Cancel() => 
        Hide();
}