using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private TextMeshProUGUI _healthBarLabel;

    private HealthBehaviour _playerHealth;

    private void Start()
    {
        var player = FindFirstObjectByType<PlayerCharacter>();
        _playerHealth = player?.GetComponent<HealthBehaviour>();

        if (_playerHealth != null)
        {
            SetHealth(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }
        else
        {
            SetHealth(0, 100);
        }
    }

    public void UpgradedHealth(int current, int max)
    {
        SetHealth(current, max);
        _playerHealth.SetMaxHealth(max);
    }

    private void Update()
    {
        Sync();
    }

    private void Sync()
    {
        if (_playerHealth == null)
        {
            SetHealth(0, 100);
        }
        else
        {
            SetHealth(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }
    }

    private void SetHealth(int current, int max)
    {
        if (_healthBar == null) return;
        _healthBar.transform.localScale = new Vector3((float)current / max, 1.0f, 1.0f);

        if (_healthBarLabel == null) return;
        _healthBarLabel.text = $"{current.ToString()}/{max.ToString()}";
    }
}