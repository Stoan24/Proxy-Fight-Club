using System;
using TMPro;
using UnityEngine;

public class InteractionMenu : MonoBehaviour
{
    public static InteractionMenu Instance;

    [SerializeField] private TextMeshProUGUI _textLabel;

    private void Awake()
    {
        Instance = this;
        Hide();
    }


    public void Show(string message)
    {
        if (_textLabel != null)
        {
            _textLabel.gameObject.SetActive(true);
            _textLabel.text = message;
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
