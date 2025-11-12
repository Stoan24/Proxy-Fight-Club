using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController _cinemachineInput;

    [SerializeField] private GameObject _startScreen;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startScreen.SetActive(true);

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        if (_startScreen == null) return;

        _startScreen.SetActive(false);

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
