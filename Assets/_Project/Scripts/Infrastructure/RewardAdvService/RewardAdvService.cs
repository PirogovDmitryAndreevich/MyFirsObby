using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class RewardAdvService : IRewardAdvService
{
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _save;
    private readonly IStaticDataService _staticData;

    public RewardAdvService(IPersistentProgressService progressService, ISaveLoadService save, IStaticDataService staticData)
    {
        _progressService = progressService;
        _save = save;
        _staticData = staticData;

        YG2.onRewardAdv += GiveReward;
    }

    public void ShowRewardAdv(RewardType rewardID) => 
        YG2.RewardedAdvShow(rewardID.ToString());

    private void GiveReward(string rewardID)
    {
        string sceneKey = SceneManager.GetActiveScene().name;
        LevelStaticData levelData = _staticData.ForLevel(sceneKey);

        if (Enum.TryParse(rewardID, out RewardType rewardType))
        {
            switch (rewardType)
            {
                case RewardType.Cash:
                    _progressService.Progress.PlayerState.AddCash(levelData.RewardCashFromAdv);
                    _save.SaveProgress();
                    break;
            }
        }
        else
        {
            return;
        }
    }
}