using UnityEngine;

public class ActorUI : MonoBehaviour
{
    [SerializeField] private HpBar _hpBar;

    private CharacterHealth _characterHealth;

    private void OnDestroy()
    {
        _characterHealth.HealthChange -= UpdateHpBar;
    }

    public void Construct(CharacterHealth health)
    {
        _characterHealth = health;

        _characterHealth.HealthChange += UpdateHpBar;
    }

    private void UpdateHpBar()
    {
        _hpBar.SetValue(_characterHealth.Current, _characterHealth.Max);
    }
}