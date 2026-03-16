using System;

[Serializable]
public class WorldData
{
    public string Level;
    public Vector3Data Position;
    private string initialLevel;

    public WorldData(string initialLevel)
    {
        Level = initialLevel;
    }
}
