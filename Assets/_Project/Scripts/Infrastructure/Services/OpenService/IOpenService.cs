using System.Collections.Generic;

public interface IOpenService : IService 
{
    Dictionary<OpeningType, IOpenObject> OpeningObjects { get; }

    void OpenOpener(OpeningType type);
    void OpenObject(OpeningType type);
    void Register(IOpenObject openObject);
}