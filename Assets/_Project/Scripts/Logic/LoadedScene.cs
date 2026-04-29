using System.Collections.Generic;

public class LoadedScene
{
    public LoadedScene(LevelStaticData levelStaticData, Character character, ConveerInitializer conveer,
        ArsenalInitializer arsenal, List<SpawnPoint> enemySpawns, Build build, Hud hud, ATM atm)
    {
        LevelData = levelStaticData;
        Character = character;
        Conveer = conveer;
        Arsenal = arsenal;
        EnemySpawns = enemySpawns;
        Build = build;
        Hud = hud;
        Atm = atm;
    }

    public LevelStaticData LevelData { get; private set; }
    public Build Build { get; private set; }
    public ATM Atm { get; private set; }
    public Character Character { get; private set; }
    public ConveerInitializer Conveer { get; private set; }
    public ArsenalInitializer Arsenal { get; private set; }
    public Hud Hud { get; private set; }
    public List<SpawnPoint> EnemySpawns { get; private set; }
}