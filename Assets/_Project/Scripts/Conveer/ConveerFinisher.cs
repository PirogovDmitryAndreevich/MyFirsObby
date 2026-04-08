using UnityEngine;

public class ConveerFinisher : MonoBehaviour
{
    [SerializeField] private TriggerObserver _trigger;
    private IPersistentProgressService _progressService;

    private void Start() => 
        _trigger.TriggerEnter += OnTrigger;

    private void OnDestroy() => 
        _trigger.TriggerEnter -= OnTrigger;

    public void Construct(IPersistentProgressService progressService) => 
        _progressService = progressService;

    private void OnTrigger(Collider collider)
    {
        if (collider.TryGetComponent(out IConveerResource resource))
        {
            _progressService.Progress.PlayerState.AddCash(resource.Reward);
            resource.Collect();
        }
    }
}