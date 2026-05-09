using UnityEngine;

public interface IAssets : IService
{
    GameObject Instantiate(string path);
    GameObject Instantiate(string path, Vector3 at);
    GameObject Instantiate(string path, Transform parent);
    TObject Instantiate<TObject>(TObject prefab) where TObject : Object;
    TObject Instantiate<TObject>(TObject prefab, Transform parent) where TObject : Object;
}