using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;

    public event Action CharacterCreated;

    public List<ISavedProgressReader> ProgressReaders { get; } = new();
    public List<ISavedProgress> ProgressWriters { get; } = new();
    public GameObject CharacterObject { get; set; }

    public GameFactory(IAssets assets)
    {
        _assets = assets;
    }

    public GameObject CreateCharacter(GameObject at)
    {
        CharacterObject = InstantiateRegistered(AssetsPath.CharacterPath, at: at.transform.position);
                
        CharacterCreated?.Invoke();

        return CharacterObject;
    }

    public GameObject CreateHudMobile() =>
        InstantiateRegistered(AssetsPath.HudMobilePath);

    public GameObject CreateHudDesktop() =>
        InstantiateRegistered(AssetsPath.HudDesktopPath);

    public void Cleanup()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
        GameObject gameObject = _assets.Instantiate(prefabPath, at);
        RegisterProgressWatchers(gameObject);

        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
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
