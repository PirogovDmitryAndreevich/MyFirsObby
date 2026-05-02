public interface IOpenObject : ISavedProgressReader
{
    OpeningType Type { get; }
    Opener Opener { get; }
    OpeningType[] NextOpeners { get; }

    void Construct(OpeningType[] next);
    void OpenObject();
    void OpenOpener();
}
