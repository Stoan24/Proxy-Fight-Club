using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
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

    public void ToggleMenu()
    {
        if (_menu == null) return;

        _isActive = !_isActive;

        _menu.SetActive(_isActive);

        if (_isActive)
        {
            OpenMenu();

            if (UpgradeReminderMenu.Instance != null)
            {
                UpgradeReminderMenu.Instance.Hide();
            }

            InteractionMenu.Instance?.Hide();
        }
        else
        {
            CloseMenu();

            if (UpgradeReminderMenu.Instance != null && _playerStats.AvailablePoints > 0)
            {
                UpgradeReminderMenu.Instance.Show();
            }
        }
    }

    private void OpenMenu()
    {
        RefreshMenu();
        GameStateManager.Instance?.MenuLock(true);

        _lookAction?.Disable();

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseMenu()
    {
        GameStateManager.Instance?.MenuLock(false);

        _lookAction?.Enable();

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
            _statsLabel.text = $"Health: {_playerStats.Health}\n" +
                               $"Strength: {_playerStats.Strength}\n" +
                               $"Speed: {_playerStats.Speed}\n";
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
