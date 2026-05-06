using UnityEngine;

public class AudioManager : ISavedProgressReader
{
    private ISaveLoadService _save;
    private float _lowPith;
    private float _topPith;

    private PlayerProgress _progress;

    public void Construct(ISaveLoadService save)
    {
        _save = save;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _progress = progress;
    }
}
