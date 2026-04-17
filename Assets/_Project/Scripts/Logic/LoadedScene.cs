using System.Collections.Generic;

public class LoadedScene
{
    public LoadedScene(Character character, ConveerInitializer conveer, ArsenalInitializer arsenal, List<SpawnPoint> enemySpawns, Build build)
    {
        Character = character;
        Conveer = conveer;
        Arsenal = arsenal;
        EnemySpawns = enemySpawns;
        Build = build;
    }

    public Build Build { get; private set; }
    public Character Character { get; private set; }
    public ConveerInitializer Conveer { get; private set; }
    public ArsenalInitializer Arsenal { get; private set; }
    public List<SpawnPoint> EnemySpawns { get; private set; }
}