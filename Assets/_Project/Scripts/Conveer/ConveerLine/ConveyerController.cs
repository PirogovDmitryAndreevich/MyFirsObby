using UnityEngine;

public class ConveyerController : MonoBehaviour
{
    [SerializeField] private ConveyerType _conveyerType;
    [SerializeField] private GameObject _conveyer;
    [SerializeField] private ConveyerFinisher _finisher;
    [SerializeField] private SpawnerResources _spawnerResources;

    public void Construct(IConveyersPool pool, ATM atm)
    {
        _spawnerResources.Construct(pool,_conveyerType);
        _finisher.Construct(atm);
    }
}