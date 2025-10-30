using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshMovementBehaviour))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    private Vector3 _previousTargetPosition;

    [SerializeField] private float _maxDistanceFromPlayer = 2.0f;

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;

        _previousTargetPosition = transform.position;
    }

    protected override void HandleMovement()
    {
        if (_target == null)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        _navMeshAgent.isStopped = false;
        _navMeshAgent.stoppingDistance = _maxDistanceFromPlayer;
        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
