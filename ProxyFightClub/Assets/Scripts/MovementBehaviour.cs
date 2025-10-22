using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] protected float _movementSpeed = 1.0f;

    [SerializeField] protected float _jumpPower = 10.0f;

    [SerializeField] protected Transform _cameraTransform;

    private Rigidbody _rigidBody;

    private Vector3 _desiredMovementDirection = Vector3.zero;
    
    protected GameObject _target;

    private bool _isGrounded = false;

    private const float GROUND_CHECK_DISTANCE = 0.2f;
    private const string GROUND_LAYER = "Ground";

    public Vector3 DesiredMovementDirection
    {
        get => _desiredMovementDirection;
        set => _desiredMovementDirection = value;
    }
    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();

        //check if ground is under player
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER));
    }
    protected virtual void HandleMovement()
    {
        if (_rigidBody == null) return;

        _rigidBody.rotation = Quaternion.Euler(_cameraTransform.eulerAngles);

        Vector3 movement = transform.forward * _desiredMovementDirection.z + transform.right * _desiredMovementDirection.x;

        movement.y = 0f;

        movement *= _movementSpeed;
        
        //remove gravity, keep y velocity
        movement.y = _rigidBody.linearVelocity.y;


        _rigidBody.linearVelocity = movement;
    }
    public void Jump()
    {
        if (_isGrounded)
        {
            _rigidBody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }
}
