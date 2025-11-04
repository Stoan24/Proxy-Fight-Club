using System;
using UnityEngine;



[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    private HUD _hud;

    public enum PlayerStatType
    {
        Health,
        Strength,
        Speed
    }

    [Header("Base Stats")] 
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _strength = 10;
    [SerializeField] private int _speed = 5;

    [Header("Increase Amount")]
    [SerializeField] private int _healthIncrease = 10;
    [SerializeField] private int _strengthIncrease = 5;
    [SerializeField] private int _speedIncrease = 2;

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
    public int Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public int AvailablePoints
    {
        get => _statpoints;
        set => _statpoints = value;
    }

    private void Awake()
    {
        _hud = FindAnyObjectByType<HUD>();
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
            case PlayerStatType.Speed:
                _speed += _speedIncrease;
                break;
            default:
                return false;
        }

        _statpoints--;
        OnStatsChanged?.Invoke();
        return true;
    }
}
