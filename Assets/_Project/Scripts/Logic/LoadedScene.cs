using System.Collections.Generic;

public class LoadedScene
{
    public LoadedScene(LevelStaticData levelStaticData, Character character,
        List<SpawnPoint> enemySpawns, Build build, Hud hud)
    {
        LevelData = levelStaticData;
        Character = character;
        EnemySpawns = enemySpawns;
        Build = build;
        Hud = hud;
    }

    public LevelStaticData LevelData { get; private set; }
    public Build Build { get; private set; }
    public Character Character { get; private set; }
    public Hud Hud { get; private set; }
    public List<SpawnPoint> EnemySpawns { get; private set; }
}