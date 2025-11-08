using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyCharacter : BasicCharacter
{
    private GameObject _playerTarget;

    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _attackCooldown;
    private float _attackTimer = 2.0f;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlinghtMaterial;

    private Renderer[] _arrRenderer;

    private NavMeshMovementBehaviour _navMesh;

    [SerializeField] private int _rewardPoints = 3;

    [SerializeField] private bool _isBoss = false;

    public int RewardPoint
    {
        get => _rewardPoints;
        set => _rewardPoints = value;
    }

    public bool IsBoss
    {
        get => _isBoss;
        set => _isBoss = value;
    }

    protected override void Awake()
    {
        base.Awake();

        //materials
        _arrRenderer = GetComponentsInChildren<Renderer>();

        foreach (var renderer in _arrRenderer)
        {
            if (renderer != null)
            {
                renderer.material = _defaultMaterial;
            }
        }

        _attackTimer = _attackCooldown;
    }
    private void Start()
    {
        var player = FindFirstObjectByType<PlayerCharacter>();
        if (player == null) return;

        _playerTarget = player.gameObject;

        _navMesh = GetComponent<NavMeshMovementBehaviour>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameStateManager.Instance == null || !GameStateManager.Instance.IsFightActive || GameStateManager.Instance.CurrentEnemy != this) return;

        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        if (_navMesh == null || _playerTarget == null) return;

        if (_movementBehaviour == null) return;

        _navMesh.Target = _playerTarget;

        Vector3 direction = _playerTarget.transform.position - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void HandleAttack()
    {
        if (_attackBehaviour == null || _playerTarget == null) return;

        //check distance from player
        var sqrDistance = (transform.position - _playerTarget.transform.position).sqrMagnitude;

        //use a timer between attacks
        _attackTimer -= Time.deltaTime;

        if (sqrDistance <= (_attackRange * _attackRange) && _attackTimer <= 0f)
        {
            bool isLeft = (Random.Range(0, 2) == 0);
            _attackBehaviour.Attack(isLeft);

            //Reset and use a random interval
            _attackTimer = Random.Range(_attackCooldown * 0.8f, _attackCooldown * 1.2f);
        }
    }

    public void SetHighlight(bool isHighlighted)
    {
        Material targetMaterial = isHighlighted ? _highlinghtMaterial : _defaultMaterial;

        foreach (var renderer in _arrRenderer)
        {
            if (renderer != null && targetMaterial != null)
            {
                renderer.material = targetMaterial;
            }
        }
    }
}
