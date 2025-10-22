using System;
using System.Numerics;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Vector3 = UnityEngine.Vector3;

public class CharacterController : MonoBehaviour
{
    private BasicCharacter _playerCharacter;
    private EnemyCharacter[] _enemyCharacterArr;
    private float _interactDistance = 3f;

    public Transform _CameraTransform
    {
        get => _CameraTransform;
        set => _CameraTransform = value;
    }
    private void Awake()
    {
        PlayerCharacter player = FindFirstObjectByType<PlayerCharacter>();

        if (player)
        {
            _playerCharacter = player;
        }

        
        EnemyCharacter[] enemies = FindObjectsByType<EnemyCharacter>(FindObjectsSortMode.None);

        foreach (var t in enemies)
        {
            if (!t) return;
        }
        _enemyCharacterArr = enemies;
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!Physics.Raycast(_CameraTransform.position, Vector3.forward, out var hitInfo, _interactDistance)) return;

        EnemyCharacter enemy = hitInfo.transform.GetComponent<EnemyCharacter>();

        enemy._isLookedAt = true;
    }
}
