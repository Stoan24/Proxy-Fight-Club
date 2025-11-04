using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private UpgradeMenu _menu;

    public void OpenMenu()
    {
        if (_menu == null) return;

        _menu.ToggleMenu();
    }
}