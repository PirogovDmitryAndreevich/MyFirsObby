using System;
using UnityEngine;


public class CharacterHealth : MonoBehaviour, ISavedProgressReader, IHealth
{
    private float _currentHealth;
    private float _maxHealth;

    public event Action HealthChanged;

    public float Current
    {
        get => _currentHealth;
        private set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                HealthChanged?.Invoke();
            }
        }
    }

    public float Max
    {
        get => _maxHealth;
        private set => _maxHealth = value;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        Max = progress.PlayerState.MaxHP;
        Current = Max;
        HealthChanged?.Invoke();
    }

    public void Heal(float value)
    {
        Current += value;

        if(Current > Max)
            Current = Max;
    }

    public void TakeDamage(float damage)
    {
        if (Current <= 0)
        {
            HealthChanged?.Invoke();
            return;
        }

        Current -= damage;
    }

    public void ResetHealth()
    {
        Current = Max;
        HealthChanged?.Invoke();
    }
}
