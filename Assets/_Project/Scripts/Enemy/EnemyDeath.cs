using System;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;

    public event Action<EnemyDeath> Happened;

    public bool IsDead { get; private set; } = false;

    private void Start() => 
        _health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
        _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
        if (_health.Current <= 0)
            Die();
    }

    private void Die()
    {
        IsDead = true;
        Happened?.Invoke(this);
    }
}
