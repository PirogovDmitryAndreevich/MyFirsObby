using UnityEngine;

public class CheckAttackRange : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;

    public bool IsAttackZone { get; private set; }

    private void OnEnable()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
    }

    private void OnDisable()
    {
        _triggerObserver.TriggerExit -= TriggerExit;
        _triggerObserver.TriggerEnter -= TriggerEnter;
    }

    private void TriggerEnter(Collider collider) =>
        IsAttackZone = true;

    private void TriggerExit(Collider collider) => 
        IsAttackZone = false;
}
