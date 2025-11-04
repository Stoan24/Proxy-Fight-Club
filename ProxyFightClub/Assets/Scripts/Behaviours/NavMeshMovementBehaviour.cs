using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshMovementBehaviour))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _previousTargetPosition;

    [SerializeField] private float _maxDistanceFromPlayer = 2.0f;

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;
    }

    protected override void HandleMovement()
    {
        if (_target == null || _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        _navMeshAgent.isStopped = false;
        _navMeshAgent.stoppingDistance = _maxDistanceFromPlayer;
        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
