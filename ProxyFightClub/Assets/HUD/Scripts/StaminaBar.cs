using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image _staminaBar;

    private StaminaBehaviour _playerStamina;

    private void Start()
    {
        _playerStamina = FindFirstObjectByType<StaminaBehaviour>();

        if (_playerStamina != null)
        {
            _playerStamina.OnStaminaChanged += SetStamina;
            SetStamina(_playerStamina.CurrentStamina, _playerStamina.MaxStamina);
        }
    }
    private void Update()
    {
        Sync();
    }

    private void Sync()
    {
        if (_staminaBar == null)
        {
            SetStamina(0, 100);
        }
        else
        {
            SetStamina(_playerStamina.CurrentStamina, _playerStamina.MaxStamina);
        }
    }

    private void SetStamina(float current, float max)
    {
        if (_staminaBar == null) return;

        _staminaBar.transform.localScale = new Vector3((float)current / max, 1.0f, 1.0f);
    }
}
