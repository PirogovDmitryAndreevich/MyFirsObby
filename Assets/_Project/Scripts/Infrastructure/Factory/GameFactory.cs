using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;

    public GameFactory(IAssets assets)
    {
        _assets = assets;
    }

    public GameObject CreateCharacter(GameObject at)
    {
        return _assets.Instantiate(AssetsPath.CharacterPath, at: at.transform.position);
    }
    public object CreateHud()
    {
        return _assets.Instantiate(AssetsPath.HudPath);
    }    
}
