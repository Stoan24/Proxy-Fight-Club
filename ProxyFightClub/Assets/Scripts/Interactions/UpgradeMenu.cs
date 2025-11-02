using System;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private TextMeshProUGUI _statsLabel;

    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = FindAnyObjectByType<PlayerStats>();

        if (_playerStats == null) return;

        _playerStats.OnStatsChanged += RefreshUI;

        if (_menu == null) return;
        
        _menu.SetActive(false);
    }


    public void ToggleMenu()
    {
        if (_menu == null) return;

        bool newState = !_menu.activeSelf;
        _menu.SetActive(newState);

        if (newState)
        {
            RefreshUI();
        }
    }

    public void UpgradeHealth()
    {
        UpgradeStat(PlayerStats.PlayerStatType.Health);
    }

    public void UpgradeStrength()
    {
        UpgradeStat(PlayerStats.PlayerStatType.Strength);
    }

    public void UpgradeSpeed()
    {
        UpgradeStat(PlayerStats.PlayerStatType.Speed);
    }

    private void UpgradeStat(PlayerStats.PlayerStatType stat)
    {
        if (_playerStats == null) return;

        if (_playerStats.UpgradeStat(stat))
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (_playerStats == null) return;

        if (_pointsLabel != null)
        {
            _pointsLabel.text = $"Points: {_playerStats.AvailablePoints}";
        }

        if (_statsLabel != null)
        {
            _statsLabel.text = $"Health: {_playerStats.Health}\n" +
                               $"Strength: {_playerStats.Strength}\n" +
                               $"Speed: {_playerStats.Speed}\n";
        }
    }
}
