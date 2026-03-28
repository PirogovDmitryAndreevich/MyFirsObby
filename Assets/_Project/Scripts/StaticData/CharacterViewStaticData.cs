using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "StaticData/Characters")]
public class CharacterViewStaticData : ScriptableObject
{
    public CharacterView[] Characters;
}