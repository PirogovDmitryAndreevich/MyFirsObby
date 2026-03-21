using System;
using UnityEngine;


public class CharacterHealth : MonoBehaviour, ISavedProgress, IHealth
{
    private State _state;

    public event Action HealthChanged;

    public float Current
    {
        get => _state.CurrentHP;
        set
        {
            if (_state.CurrentHP != value)
            {
                _state.CurrentHP = value;
                HealthChanged?.Invoke();
            }
        }
    }

    public float Max
    {
        get => _state.MaxHP;
        set => _state.MaxHP = value;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.CharacterState;
        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.CharacterState.CurrentHP = Current;
        progress.CharacterState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
        if (Current <= 0)
            return;

        Current -= damage;
    }
}
