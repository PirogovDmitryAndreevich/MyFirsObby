using System;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private CharacterHealth _health;
    private bool _isDead = false;

    private void Start() => 
        _health.HealthChange += HealthChanged;

    private void OnDestroy() => 
        _health.HealthChange -= HealthChanged;

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
    }
}
