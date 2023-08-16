using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler onDeath;
    [SerializeField] private int _health = 100;

    public void TakeDamage(int dmgAmount)
    {
        _health -= dmgAmount;
        Debug.Log(_health);
        // Prevent health from falling below 0
        if (_health < 0) _health = 0;
        if (_health == 0) Die();
    }

    private void Die()
    {
        onDeath?.Invoke(this, EventArgs.Empty);
    }
}
