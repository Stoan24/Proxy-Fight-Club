using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    public void OpenMenu()
    {
        if (_menu == null) return;
        
        _menu.SetActive(!_menu.activeSelf);
    }
}
