using System;

[Serializable]
public class PlayerProgress
{
    public WorldData WorldData;
    public PlayerResources Resources;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        Resources = new PlayerResources();
    }
}

public class PlayerResources
{
    public int Score;
    public int Cash;
}