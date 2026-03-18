using System.Collections;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;
    [SerializeField] private Follow _followComponent;
    [SerializeField] private float _followCooldown;

    private bool _hasAggroTarget;
    private Coroutine _cooldownCoroutine;

    private void Start()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;

        SwitchFollowOff();
    }

    private void OnDestroy()
    {
        _triggerObserver.TriggerExit -= TriggerExit;
        _triggerObserver.TriggerEnter -= TriggerEnter;
    }

    private void TriggerEnter(Collider collider)
    {
        if (_hasAggroTarget)
            return;

            _hasAggroTarget = true;

            StopAggroCoroutine();
            SwitchFollowOn();
    }   

    private void TriggerExit(Collider collider)
    {
        if (!_hasAggroTarget)
            return;
        
            _hasAggroTarget = false;
            _cooldownCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown()); 
    }

    private void SwitchFollowOff() => 
        _followComponent.enabled = false;

    private void SwitchFollowOn() => 
        _followComponent.enabled = true;

    private void StopAggroCoroutine()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }       
    }

    private IEnumerator SwitchFollowOffAfterCooldown()
    {
        yield return new WaitForSeconds(_followCooldown);

        SwitchFollowOff();
    }
}