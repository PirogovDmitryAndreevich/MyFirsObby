using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateCharacter(GameObject at);
    object CreateHud();
}