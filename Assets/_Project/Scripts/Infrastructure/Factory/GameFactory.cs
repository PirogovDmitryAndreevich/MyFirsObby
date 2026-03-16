using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;

    public List<ISavedProgressReader> ProgressReaders { get; } = new();
    public List<ISavedProgress> ProgressWriters { get; } = new();

    public GameFactory(IAssets assets)
    {
        _assets = assets;
    }

    public GameObject CreateCharacter(GameObject at) =>
        InsatiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);

    public void CreateHud() =>
        InsatiateRegistered(AssetsPath.HudPath);

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }

    private GameObject InsatiateRegistered(string prefabPath, Vector3 at)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath, at);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InsatiateRegistered(string prefabPath)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
    }

    private void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is ISavedProgress progressWriter)
            ProgressWriters.Add(progressWriter);

        ProgressReaders.Add(progressReader);
    }
}
