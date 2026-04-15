using UnityEngine;

public class CheckAttackRange : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;

    public bool AttackRange { get; private set; }

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
        AttackRange = true;

    private void TriggerExit(Collider collider) => 
        AttackRange = false;
}
