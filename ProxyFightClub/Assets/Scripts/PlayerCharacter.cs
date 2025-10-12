using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private InputActionReference _movementAction;

    private InputAction _jumpAction;
    private InputAction _handAction;

    protected override void Awake()
    {
        base.Awake();
        if (_inputAsset == null) return;

        //example of searching for the bindings in code
        _jumpAction = _inputAsset.FindActionMap("Movement").FindAction("Jump");
        _handAction = _inputAsset.FindActionMap("Hands").FindAction("Left");

        //we bind a callback to it instead of continiously monitoring input
        _jumpAction.performed += HandleJumpInput;
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
        if (_attackBehaviour == null || _handAction == null) return;

        
        if (_handAction.WasPressedThisFrame()) _attackBehaviour.Attack();
    }
    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
    }
}