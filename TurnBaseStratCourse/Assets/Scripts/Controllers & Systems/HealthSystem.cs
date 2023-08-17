using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 100;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(int dmgAmount)
    {
        _currentHealth -= dmgAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        // Prevent health from falling below 0
        if (_currentHealth < 0) _currentHealth = 0;
        if (_currentHealth == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        // Returns the current health value normalized (from a range of 0 - 1)

        // Force a float to be returned by casting one term to float when calculating
        return (float)_currentHealth /_maxHealth;
    }
}
