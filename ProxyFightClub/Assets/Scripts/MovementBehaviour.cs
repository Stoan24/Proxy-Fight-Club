using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _jumpPower = 10.0f;

    [SerializeField]
    private Transform _cameraTransform;

    private Rigidbody _rigidBody;

    private Vector3 _desiredMovementDirection = Vector3.zero;

    private bool _isGrounded = false;

    private const float GROUND_CHECK_DISTANCE = 0.2f;
    private const string GROUND_LAYER = "Ground";

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        //check if ground is under player
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER));
    }
    private void HandleMovement()
    {
        if (_rigidBody == null) return;

        Vector3 movement = _cameraTransform.forward * _desiredMovementDirection.z +
                           _cameraTransform.right * _desiredMovementDirection.x;
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
