using System;
using UnityEngine;
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
        if (Enum.TryParse(rewardID, out RewardType rewardType))
        {
            switch (rewardType)
            {
                case RewardType.Cash:
                    Debug.Log("Cash reward added");
                    break;
            }
        }
        else
        {
            return;
        }
    }
}