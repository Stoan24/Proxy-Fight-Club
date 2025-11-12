using System;
using UnityEngine;

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    private PlayerHealthBar _hud;

    public enum PlayerStatType
    {
        Health,
        Strength,
        Stamina
    }

    [Header("Base Stats")] 
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _strength = 10;
    [SerializeField] private int _stamina = 20;

    private int _dodgeUnlockTreshhold = 30;
    private bool _canDodge = false;

    [Header("Increase Amount")]
    [SerializeField] private int _healthIncrease = 10;
    [SerializeField] private int _strengthIncrease = 5;
    [SerializeField] private int __staminaIncrease = 5;

    [SerializeField] private int _statpoints = 0;

    public event Action OnStatsChanged;

    public int Health
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }
    public int Strength
    {
        get => _strength;
        set => _strength = value;
    }
    public int Stamina
    {
        get => _stamina;
        set => _stamina = value;
    }

    public int AvailablePoints
    {
        get => _statpoints;
        set => _statpoints = value;
    }
    public bool CanDodge
    {
        get => _canDodge;
        set => _canDodge = value;
    }

    private void Awake()
    {
        _hud = FindAnyObjectByType<PlayerHealthBar>();

        _canDodge = false;
    }

    public void AddStatPoints(int amount)
    {
        _statpoints += amount;
        OnStatsChanged?.Invoke();
    }

    public bool UpgradeStat(PlayerStatType stat)
    {
        if (_statpoints <= 0) return false;

        switch (stat)
        {
            case PlayerStatType.Health:
                _maxHealth += _healthIncrease;
                _hud.UpgradedHealth(_maxHealth, _maxHealth);
                break;
            case PlayerStatType.Strength:
                _strength += _strengthIncrease;
                break;
            case PlayerStatType.Stamina:
                _stamina += __staminaIncrease;
                break;
            default:
                return false;
        }

        if (_stamina >= _dodgeUnlockTreshhold && !_canDodge)
        {
            _canDodge = true;
        }

        _statpoints--;
        OnStatsChanged?.Invoke();
        return true;
    }
}
