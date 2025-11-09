using System;
using UnityEngine;

public class StaminaBehaviour : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float _maxStamina = 20f;
    [SerializeField] private float _currentStamina = 20f;
    [SerializeField] private float _regenRate = 5f;
    [SerializeField] private float _regenDelay = 1.0f;

    private float _regenTimer;

    public float CurrentStamina
    {
        get => _currentStamina;
        set => _currentStamina = value;
    }

    public float MaxStamina
    {
        get => _maxStamina;
        set => _maxStamina = value;
    }

    public event Action<float, float> OnStaminaChanged; // current, max

    private void FixedUpdate()
    {
        if (_currentStamina < _maxStamina)
        {
            _regenTimer -= Time.deltaTime;

            if (_regenTimer <= 0f)
            {
                _currentStamina += _regenRate * Time.deltaTime;
                _currentStamina = Mathf.Min(_currentStamina, _maxStamina);
                OnStaminaChanged?.Invoke(_currentStamina, _maxStamina);
            }
        }
    }

    // Call when the player spends stamina
    public bool SpendStamina(float amount)
    {
        if (_currentStamina < amount) return false;

        _currentStamina -= amount;
        _regenTimer = _regenDelay;
        OnStaminaChanged?.Invoke(_currentStamina, _maxStamina);
        return true;
    }

    public void SetStamina(float stamina)
    {
        _maxStamina = stamina;
    }
}
