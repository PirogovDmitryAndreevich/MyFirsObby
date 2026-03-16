using System.Collections.Generic;
using UnityEngine;

public interface IGameFactory : IService
{
    List<ISavedProgress> ProgressWriters { get; }
    List<ISavedProgressReader> ProgressReaders { get; }

    void Cleanup();
    GameObject CreateCharacter(GameObject at);
    void CreateHud();
}