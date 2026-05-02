using System;
using UnityEngine;

[Serializable]
public class PlayerProgress
{
    public string MainScene = "Main";

    public PlayerState PlayerState;
    public Arsenal Arsenal;
    public BuildProgress BuildProgress;

    public PlayerProgress(IStaticDataService staticData)
    {
        PlayerState = new PlayerState(staticData);
        Arsenal = new Arsenal(staticData);
        BuildProgress = new BuildProgress();
    }
}
