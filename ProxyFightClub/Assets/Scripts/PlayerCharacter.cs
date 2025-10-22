using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _rightHandAction;
    [SerializeField] private InputActionReference _leftHandAction;
    
    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _jumpAction;

    [SerializeField] private InputActionReference _interaction;

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
        //_hasEnemy = _interactBehaviour.CheckIfInRange(_cameraTransform.position);


        if (_interaction.ToInputAction().WasPerformedThisFrame())
        {
            
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