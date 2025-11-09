using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshMovementBehaviour))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _previousTargetPosition;

    [SerializeField] private float _maxDistanceFromPlayer = 2.0f;


    [Header("AI Settings")]
    [SerializeField] private float _pauseChance = 0.15f;      
    [SerializeField] private float _sidestepChance = 0.25f;   
    [SerializeField] private float _pauseDuration = 1f;       
    [SerializeField] private float _sidestepDistance = 2f;    
    [SerializeField] private float _decisionInterval = 2f;    

    private float _decisionTimer;
    private bool _isPaused;
    private bool _isSidestepping;

    private Vector3 _sidestepTarget;

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
        _navMeshAgent.stoppingDistance = _maxDistanceFromPlayer;
    }

    protected override void HandleMovement()
    {
        if (_target == null || !_navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        //decision timer
        _decisionTimer -= Time.deltaTime;
        if (_decisionTimer <= 0f)
        {
            _decisionTimer = _decisionInterval;
            DecideNextMove();
        }

        if (_isPaused)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        if (_isSidestepping)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_sidestepTarget);

            if (Vector3.Distance(transform.position, _sidestepTarget) < 0.5f)
            {
                _isSidestepping = false;
            }
            return;
        }

        //default = move
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_target.transform.position);
    }

    private void DecideNextMove()
    {
        float rand = Random.value;

        if (rand < _pauseChance)
        {
            _isPaused = true;
            _isSidestepping = false;
            Invoke(nameof(EndPause), _pauseDuration);
        }
        else if (rand < _pauseChance + _sidestepChance)
        {
            Vector3 right = transform.right;
            Vector3 sideDirection = Random.value > 0.5f ? right : -right;

            _sidestepTarget = transform.position + sideDirection * _sidestepDistance;

            _isSidestepping = true;
            _isPaused = false;
        }
        else
        {
            _isPaused = false;
            _isSidestepping = false;
        }
    }

    private void EndPause()
    {
        _isPaused = false;
    }
}
