using UnityEngine;

public class ConveerFinisher : MonoBehaviour
{
    [SerializeField] private TriggerObserver _trigger;
    private ATM _atm;

    private void Start() => 
        _trigger.TriggerEnter += OnTrigger;

    private void OnDestroy() => 
        _trigger.TriggerEnter -= OnTrigger;

    public void Construct(ATM atm) => 
       _atm = atm;

    private void OnTrigger(Collider collider)
    {
        if (collider.TryGetComponent(out IConveerResource resource))
        {
            _atm.AddCash(resource.Reward);
            resource.Collect();
        }
    }
}