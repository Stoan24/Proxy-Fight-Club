using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TextMeshProUGUI _pointsLabel;
    [SerializeField] private TextMeshProUGUI _statsLabel;

    [SerializeField] private CinemachineInputAxisController _cinemachineInput;
    private InputAction _lookAction;

    private bool _isActive = false;

    private PlayerStats _playerStats;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Awake()
    {
        _playerStats = FindAnyObjectByType<PlayerStats>();
        if (_playerStats != null)
        {
            _playerStats.OnStatsChanged += RefreshMenu;
        }

        if (_menu != null)
        {
            _menu.SetActive(false);
            _isActive = false;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenMenu()
    {
        if (_menu == null) return;

        _isActive = true;
        _menu.SetActive(_isActive);

        RefreshMenu();

        _lookAction?.Disable();

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpgradeReminderMenu.Instance?.Hide();
        InteractionMenu.Instance?.Hide();
    }

    public void CloseMenu()
    {
        if (_menu == null) return;

        _isActive = false;
        _menu.SetActive(_isActive);

        _lookAction?.Enable();

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (UpgradeReminderMenu.Instance != null && _playerStats.AvailablePoints > 0)
        {
            UpgradeReminderMenu.Instance.Show();
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

    public void UpgradeStamina()
    {
        UpgradeStat(PlayerStats.PlayerStatType.Stamina);
    }

    private void UpgradeStat(PlayerStats.PlayerStatType stat)
    {
        if (_playerStats == null) return;

        if (_playerStats.UpgradeStat(stat))
        {
            RefreshMenu();
        }
    }

    private void RefreshMenu()
    {
        if (_playerStats == null) return;

        if (_pointsLabel != null)
        {
            _pointsLabel.text = $"Points: {_playerStats.AvailablePoints}";
        }

        if (_statsLabel != null)
        {
            _statsLabel.text = $"Health: {_playerStats.Health}\n \n" +
                               $"Strength: {_playerStats.Strength}\n \n" +
                               $"Stamina: {_playerStats.Stamina}\n \n";
        }
    }

    private void OnDestroy()
    {
        if (_playerStats != null)
        {
            _playerStats.OnStatsChanged -= RefreshMenu;
        }
    }
}
