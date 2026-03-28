using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveer : MonoBehaviour
{
    [SerializeField] private ConveerCreator[] _conveerCreators;

    private Dictionary<ConveerNumbers, ConveerCreator> _creators;

    public void Init() =>
        _creators = _conveerCreators.ToDictionary(x => x.Numbers, x => x);

    public void InitLine(ConveerNumbers number, bool isOpened, float cooldown, GameObject prefab)
    {
        _creators[number].gameObject.SetActive(isOpened);

        if (!isOpened)
            return;

        _creators[number].Cooldown = cooldown;
        _creators[number].Prefab = prefab;
    }
}
