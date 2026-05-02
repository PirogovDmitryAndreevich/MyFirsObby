using System.Collections.Generic;

public class OpenService : IOpenService
{
    private readonly ISaveLoadService _save;
    private readonly IPersistentProgressService _progressService;

    public Dictionary<OpeningType, IOpenObject> OpeningObjects { get; private set; }

    public OpenService(ISaveLoadService save, IPersistentProgressService progressService)
    {
        OpeningObjects = new();
        _save = save;
        _progressService = progressService;
    }

    public void Register(IOpenObject openObject)
    {
        if (!OpeningObjects.ContainsKey(openObject.Type))
            OpeningObjects[openObject.Type] = openObject;
    }

    public void OpenOpener(OpeningType type)
    {
        OpenNextOpener(type);
        _save.SaveProgress();
    }

    public void OpenObject(OpeningType type)
    {
        IOpenObject obj = OpeningObjects[type];

        _progressService.Progress.BuildProgress.OpenedOpeners.Remove(type);
        obj.Opener.gameObject.SetActive(false);

        obj.OpenObject();
        _progressService.Progress.BuildProgress.OpenedObjects.Add(type);

        foreach (OpeningType nextType in obj.NextOpeners)
            OpenNextOpener(nextType);

        _save.SaveProgress();
    }
    private void OpenNextOpener(OpeningType type)
    {
        _progressService.Progress.BuildProgress.OpenedOpeners.Add(type);
        OpeningObjects[type].OpenOpener();
    }
}