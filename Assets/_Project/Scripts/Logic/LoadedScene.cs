public class LoadedScene
{
    public LoadedScene(Character character, ConveerInitializer conveer, ArsenalInitializer arsenal)
    {
        Character = character;
        Conveer = conveer;
        Arsenal = arsenal;
    }

    public Character Character { get; private set; }
    public ConveerInitializer Conveer { get; private set; }
    public ArsenalInitializer Arsenal { get; private set; }
}