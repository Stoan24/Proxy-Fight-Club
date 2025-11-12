using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private UpgradeMenu _menu;

    public void OpenMenu()
    {
        if (_menu == null) return;

        _menu.OpenMenu();
    }

    public void CloseMenu()
    {
        if (_menu == null) return;

        _menu.CloseMenu();
    }
}