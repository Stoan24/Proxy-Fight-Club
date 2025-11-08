using UnityEngine;
using System;
using UnityEngine.AI;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public int currentHealth { get { return _currentHealth; } }
    public int maxHealth { get { return _maxHealth; } }

    public event Action OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(0, _currentHealth);

        if (_currentHealth > 0f) return;

        OnDeath?.Invoke();
        Die();
    }
    public void HealFull()
    {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(int amount)
    {
        _maxHealth = amount;
        HealFull();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}