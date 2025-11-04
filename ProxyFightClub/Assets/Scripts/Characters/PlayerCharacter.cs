using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _rightHandAction;
    [SerializeField] private InputActionReference _leftHandAction;
    
    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _dodgeAction;

    [SerializeField] private InputActionReference _interaction;
    [SerializeField] private InputActionReference _escape;
    [SerializeField] private float _interactDistance = 5f;

    private EnemyCharacter _currentEnemy;
    private const string ENEMY_LAYER = "Enemy";

    private UpgradeStation _currentStation;
    private PlayerStats _stats;

    [SerializeField] private Transform _cameraTransform;

    protected override void Awake()
    {
        base.Awake();
        _stats = GetComponent<PlayerStats>();
        if (_inputAsset == null) return;

        _dodgeAction.ToInputAction().performed += HandleDodgeInput;
    }

    private void Start()
    {
        ApplyStats();

        if (_stats != null)
        {
            _stats.OnStatsChanged += ApplyStats;
        }
    }

    private void ApplyStats()
    {
        if (_stats == null) return;

        if (_healthBehaviour != null)
        {
            _attackBehaviour.SetDamage(_stats.Strength);
        }

        if (_movementBehaviour != null)
        {
            _movementBehaviour.SetSpeed(_stats.Speed);
        }
    }

    private void OnEnable()
    {
        if (_inputAsset == null) return;
        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;
        _inputAsset.Disable();
    }

    private void Update()
    {
        if (_escape.ToInputAction().WasPerformedThisFrame())
        {
            if (_currentStation != null)
            {
                _currentStation.OpenMenu();
                _currentStation = null;
            }
        }

        if (GameStateManager.Instance != null && GameStateManager.Instance.IsInMenu) return;

        HandleMovementInput();
        HandleAttackInput();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (_interaction == null || _cameraTransform == null) return;


        //rayCast forward from camera
        if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hitInfo, _interactDistance))
        {
            RemoveEnemy();
            return;
        }

        // -- ENEMY --

        var enemy = hitInfo.transform.GetComponentInParent<EnemyCharacter>();
        if (enemy != null)
        {
            if (_currentEnemy != enemy)
            {
                RemoveEnemy();
            }

            _currentEnemy = enemy;
            _currentEnemy.SetHighlight(true);

            //interaction
            if (_interaction.ToInputAction().WasPerformedThisFrame() && GameStateManager.Instance != null)
            {
                GameStateManager.Instance.StartFight(enemy);
            }

            return;
        }
        RemoveEnemy();

        // -- UPGRADE STATION --
        if (hitInfo.transform.TryGetComponent<UpgradeStation>(out var station))
        {
            if (_interaction.ToInputAction().WasPerformedThisFrame())
            {
                _currentStation = station;
                _currentStation.OpenMenu();
            }
        }
    }

    private void RemoveEnemy()
    {
        if (_currentEnemy != null)
        {
            _currentEnemy.SetHighlight(false);
            _currentEnemy = null;
        }
    }

    void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null) return;

        //movement
        Vector2 movementInput = _movementAction.action.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(movementInput.x, 0 , movementInput.y);

        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleDodgeInput(InputAction.CallbackContext context)
    {
        if (_movementBehaviour == null) return;

        Vector2 moveInput = _movementAction.action.ReadValue<Vector2>();

        _movementBehaviour.Dodge(moveInput);
    }

    private void HandleAttackInput()
    {
        if (_attackBehaviour == null || _rightHandAction == null || _leftHandAction == null) return;

        if (_rightHandAction.ToInputAction().WasPerformedThisFrame())
        {
            _attackBehaviour.Attack(false);
        }
        else if (_leftHandAction.ToInputAction().WasPerformedThisFrame())
        {
            _attackBehaviour.Attack(true);
        }
    }

    protected void OnDestroy()
    {
        if (_stats != null)
        {
            _stats.OnStatsChanged -= ApplyStats;
        }
        _dodgeAction.ToInputAction().performed -= HandleDodgeInput;
    }
}