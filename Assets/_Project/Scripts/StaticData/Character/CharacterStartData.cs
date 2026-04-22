using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/CharactersData")]
public class CharacterStartData : ScriptableObject
{
    public CharacterType CharacterViewType;
    public int Score;
    public int Cash;
    public float MaxHP;
}