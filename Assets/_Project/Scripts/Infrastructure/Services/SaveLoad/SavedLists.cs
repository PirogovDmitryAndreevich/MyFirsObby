using System.Collections.Generic;

public class SavedLists : ISavedLists
{
    public List<ISavedProgressReader> ProgressReaders { get; }
    public List<ISavedProgress> ProgressWriters { get; }

    public SavedLists()
    {
        ProgressReaders = new List<ISavedProgressReader>();
        ProgressWriters = new List<ISavedProgress>();
    }

    public void AddReader(ISavedProgressReader reader) => 
        ProgressReaders.Add(reader);

    public void AddWriter(ISavedProgress writer) => 
        ProgressReaders.Add(writer);

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }
}