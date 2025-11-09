using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float _movementSpeed = 5.0f;
    [SerializeField] protected Transform _cameraTransform;

    [Header("Dodge Settings")]
    [SerializeField] protected float _dodgePower = 10f;
    [SerializeField] private float _dodgeCooldown = 1f;
    [SerializeField] private float _dodgeDuration = 0.2f;
    [SerializeField] private float _staminaCost = 5f;

    private float _dodgeTimer;
    private bool _isDodging = false;

    private StaminaBehaviour _stamina;

    private Rigidbody _rigidBody;

    private Vector3 _desiredMovementDirection = Vector3.zero;
    
    protected GameObject _target;

    public Vector3 DesiredMovementDirection
    {
        get => _desiredMovementDirection;
        set => _desiredMovementDirection = value;
    }

    public bool IsDodging
    {
        get => _isDodging;
        set => _isDodging = value;
    }

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _stamina = GetComponent<StaminaBehaviour>();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();

        if (_dodgeTimer > 0f)
        {
            _dodgeTimer -= Time.deltaTime;
        }
    }
    protected virtual void HandleMovement()
    {
        if (_rigidBody == null || IsDodging) return;

        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0;

        _rigidBody.rotation = Quaternion.Euler(0, Quaternion.LookRotation(cameraForward).eulerAngles.y, 0);

        var movement = transform.forward * _desiredMovementDirection.z + transform.right * _desiredMovementDirection.x;
        movement.y = 0f;
        movement *= _movementSpeed;
        
        //remove gravity, keep y velocity
        movement.y = _rigidBody.linearVelocity.y;

        _rigidBody.linearVelocity = movement;
    }
    public void Dodge(Vector2 moveInput)
    {
        if (_stamina != null && !_stamina.SpendStamina(_staminaCost)) return;

        if (_isDodging || _dodgeTimer > 0f) return;

        _dodgeTimer = _dodgeCooldown;
        _isDodging = true;

        //get diraction based of movement
        Vector3 dodgeDirection = new Vector3(moveInput.x, 0.5f, moveInput.y);

        if (dodgeDirection.sqrMagnitude < 0.01f)
        {
            dodgeDirection = Vector3.forward;
        }

        Vector3 worldDirection = (_cameraTransform.forward.normalized * dodgeDirection.z + _cameraTransform.right.normalized * dodgeDirection.x).normalized;


        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.AddForce(worldDirection * _dodgePower, ForceMode.Impulse);
        
        Invoke(nameof(EndDodge), _dodgeDuration);
    }

    private void EndDodge()
    {
        _isDodging = false;
    }
}
