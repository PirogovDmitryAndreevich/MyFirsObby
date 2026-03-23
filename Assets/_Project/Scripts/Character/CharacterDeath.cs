using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private CharacterHealth _health;
    private bool _isDead = false;

    private void Start() => 
        _health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
        _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (!_isDead && _health.Current <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("Character died");
    }
}
