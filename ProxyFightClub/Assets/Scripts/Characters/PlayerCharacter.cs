using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _rightHandAction;
    [SerializeField] private InputActionReference _leftHandAction;
    
    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _jumpAction;

    [SerializeField] private InputActionReference _interaction;
    [SerializeField] private float _interactDistance = 5f;

    private EnemyCharacter _currentEnemy;
    private const string ENEMY_LAYER = "Enemy";

    private UpgradeStation _currentStation;

    [SerializeField] private Transform _cameraTransform;

    protected override void Awake()
    {
        base.Awake();
        if (_inputAsset == null) return;

        _jumpAction.ToInputAction().performed += HandleJumpInput;
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
            if (_interaction.ToInputAction().WasPerformedThisFrame())
            {
                if (GameStateManager.Instance != null)
                {
                    GameStateManager.Instance.StartFight(enemy);
                }
            }
        }

        // -- UPGRADE STATION --
        if (!hitInfo.transform.TryGetComponent<UpgradeStation>(out var station)) return;

        _currentStation = station;

        if (_interaction.ToInputAction().WasPerformedThisFrame())
        {
            _currentStation.OpenMenu();
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

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (_movementBehaviour == null) return;

        _movementBehaviour.Jump();
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
        _jumpAction.ToInputAction().performed -= HandleJumpInput;
    }
}