using System;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private CharacterHealth _health;
    private bool _isDead = false;

    public event Action CharacterDead;

    private void Start() => 
        _health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
        _health.HealthChanged -= HealthChanged;

    public void ResetState() => 
        _isDead = false;

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
        CharacterDead?.Invoke();
        Debug.Log("Character died");
    }
}
