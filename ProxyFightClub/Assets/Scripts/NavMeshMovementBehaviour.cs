using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _previousTargetPosition = Vector3.zero;

    private const float _movementEpsilon = .25f;

    [SerializeField] private float _maxDistanceFromPlayer = 2.0f;

    private bool _hasStopped = false;

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

        float distanceToPlayer = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToPlayer <= _maxDistanceFromPlayer)
        {
            if (!_hasStopped)
            {
                //stop the movement
                _navMeshAgent.SetDestination(transform.position);
                _navMeshAgent.isStopped = true;
                _hasStopped = true;
            }
        }

        if (!((_target.transform.position - _previousTargetPosition).sqrMagnitude > _movementEpsilon)) return;

        
        _navMeshAgent.isStopped = false;

        _navMeshAgent.SetDestination(_target.transform.position);
        _previousTargetPosition = _target.transform.position;

        _hasStopped = false;
    }
}
