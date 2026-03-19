using System;

[Serializable]
public class PlayerProgress
{
    public WorldData WorldData;
    public PlayerResources Resources;
    public State CharacterState;

    public  PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        Resources = new PlayerResources();
        CharacterState = new State();
    }
}

[Serializable]
public class PlayerResources
{
    public int Score;
    public int Cash;
}

[Serializable]
public class State
{
    public float CurrentHP;
    public float MaxHP;

    public void ResetHP() => CurrentHP = MaxHP;
}