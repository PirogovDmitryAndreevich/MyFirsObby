using System.Collections.Generic;

public interface ISavedLists : IService
{
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }

    public void AddReader(ISavedProgressReader reader);

    public void AddWriter(ISavedProgress writer);

    public void Cleanup();
}