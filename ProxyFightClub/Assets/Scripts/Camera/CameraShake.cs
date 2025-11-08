using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] private float _shakeIntensity = 1f;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        Instance = this;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        if (_impulseSource == null) return;
        
        _impulseSource.GenerateImpulse(_shakeIntensity);
    }
}
