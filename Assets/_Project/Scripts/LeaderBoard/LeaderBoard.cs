using UnityEngine;
using YG;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderboardYG _leaderboard;
    private IPersistentProgressService _progressService;

    private void OnDestroy()
    {
        _progressService.Progress.PlayerState.ScoreChanged -= SetNewValueLB;
    }

    public void Construct(IPersistentProgressService progressService)
    {
        _progressService = progressService;
        _progressService.Progress.PlayerState.ScoreChanged += SetNewValueLB;

        SetNewValueLB();
    }

    private void SetNewValueLB()
    {
        _leaderboard.SetLeaderboard(_progressService.Progress.PlayerState.Score);
        _leaderboard.UpdateLB();
    }
}
