using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;

    [SerializeField] private float _attackRange = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerCharacter player = FindFirstObjectByType<PlayerCharacter>();

        if (player)
        {
            _playerTarget = player.gameObject;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMomevement();
        HandleAttack();
    }

    void HandleMomevement()
    {
        if (_movementBehaviour == null) return;

        _movementBehaviour.Target = _playerTarget;
    }

    void HandleAttack()
    {
        if (_attackBehaviour == null) return;
        if (_playerTarget == null) return;

        if ((transform.position - _playerTarget.transform.position).sqrMagnitude < (_attackRange * _attackRange))
        {
            int number = Random.Range(0, 1);
            bool isLeft = (number == 0);

            _attackBehaviour.Attack(isLeft);
        }
    }
}
