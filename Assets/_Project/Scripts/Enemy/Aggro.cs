using UnityEngine;

public class Aggro : MonoBehaviour
{
    [SerializeField] private TriggerObserver _triggerObserver;

    public bool HasAggroTarget { get; private set; }
    public Transform CharacterTransform { get; private set; }

    private void Start()
    {
        _triggerObserver.TriggerEnter += TriggerEnter;
        _triggerObserver.TriggerExit += TriggerExit;
    }

    private void OnDestroy()
    {
        _triggerObserver.TriggerExit -= TriggerExit;
        _triggerObserver.TriggerEnter -= TriggerEnter;
    }

    private void TriggerEnter(Collider collider)
    {
        if (HasAggroTarget)
            return;

        if (collider.TryGetComponent<Character>(out Character character))
        {
            HasAggroTarget = true;
            CharacterTransform = character.transform;
        }
    }

    private void TriggerExit(Collider collider)
    {
        if (!HasAggroTarget)
            return;

        HasAggroTarget = false;
        CharacterTransform = null;
    }
}