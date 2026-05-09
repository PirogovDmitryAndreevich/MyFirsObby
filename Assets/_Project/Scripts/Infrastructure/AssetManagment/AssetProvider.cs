using UnityEngine;

public class AssetProvider : IAssets
{
    public GameObject Instantiate(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab);
    }

    public GameObject Instantiate(string path, Vector3 at)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, at, Quaternion.identity);
    }

    public GameObject Instantiate(string path, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, parent);
    }

    public TObject Instantiate<TObject>(TObject prefab)  where TObject : Object =>
        Object.Instantiate(prefab);

    public TObject Instantiate<TObject>(TObject prefab, Transform parent) where TObject : Object =>
        Object.Instantiate(prefab, parent);
}