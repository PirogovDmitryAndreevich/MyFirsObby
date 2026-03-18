using System;
using UnityEngine;

[RequireComponent(typeof(Attack))]
public class CheckAttackRange : MonoBehaviour
{
    [SerializeField] private Attack _attack;
    [SerializeField] private TriggerObserver _triggerObserver;

    private void Start()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;

        _attack.DisableAttack();
    }

    private void OnDestroy()
    {
        _triggerObserver.TriggerExit -= TriggerExit;
        _triggerObserver.TriggerEnter -= TriggerEnter;
    }

    private void TriggerEnter(Collider collider) => 
        _attack.EnableAttack();

    private void TriggerExit(Collider collider) => 
        _attack.DisableAttack();
}
