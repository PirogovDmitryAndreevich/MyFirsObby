using System;

public interface IHealth
{
    float Current { get;}
    float Max { get;}

    event Action HealthChanged;

    void Heal(float value);

    void TakeDamage(float damage);
}