using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionMenu : MonoBehaviour
{
    public static InteractionMenu Instance;

    [SerializeField] private TextMeshProUGUI _interactionLabel;

    private void Awake()
    {
        Instance = this;
        Hide();
    }


    public void Show(string message)
    {
        if (_interactionLabel != null)
        {
            _interactionLabel.gameObject.SetActive(true);
            _interactionLabel.text = message;
        }
    }

    public void Hide()
    {
        if (_interactionLabel != null)
        {
            _interactionLabel.gameObject.SetActive(false);
        }
    }
}
