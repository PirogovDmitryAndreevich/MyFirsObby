using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFactory : IService
{
    List<ISavedProgress> ProgressWriters { get; }
    List<ISavedProgressReader> ProgressReaders { get; }

    // Refactor
    GameObject CharacterObject { get; }
    event Action CharacterCreated;

    void Cleanup();
    GameObject CreateCharacter(GameObject at);
    GameObject CreateHudMobile();
    GameObject CreateHudDesktop();
}