using System;
using UnityEngine;


public class CharacterHealth : MonoBehaviour, ISavedProgress
{
    private State _state;

    public event Action HealthChange;

    public float Current
    {
        get => _state.CurrentHP;
        set
        {
            if (_state.CurrentHP != value)
            {
                _state.CurrentHP = value;
                HealthChange?.Invoke();
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
        HealthChange?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.CharacterState.CurrentHP = Current;
        progress.CharacterState.MaxHP = Max;
    }

    public void TakeDame(float damage)
    {
        if (Current <= 0)
            return;

        Current -= damage;
    }
}
