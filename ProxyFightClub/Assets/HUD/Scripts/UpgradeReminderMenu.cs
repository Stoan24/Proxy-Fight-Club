using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class UpgradeReminderMenu : MonoBehaviour
{
    public static UpgradeReminderMenu Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _textLabel;
    [SerializeField] private string _reminderText = "You have unspent upgrade points! Visit a PC to upgrade.";

    private PlayerStats _playerStats;


    private void Start()
    {
        _playerStats = FindAnyObjectByType<PlayerStats>();

        if (_playerStats != null)
        {
            _playerStats.OnStatsChanged += Refresh;
        }

        Hide();
    }

    private void OnDestroy()
    {
        if (_playerStats != null)
        {
            _playerStats.OnStatsChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        if (_playerStats == null) return;

        if (_playerStats.AvailablePoints > 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        if (_textLabel != null)
        {
            _textLabel.gameObject.SetActive(true);
            _textLabel.text = _reminderText;
        }
    }

    public void Hide()
    {
        if (_textLabel != null)
        {
            _textLabel.gameObject.SetActive(false);
        }
    }
}
