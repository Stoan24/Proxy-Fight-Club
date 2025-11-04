using UnityEngine;
using UnityEngine.AI;

public class BasicCharacter : MonoBehaviour
{
    protected AttackBehaviour _attackBehaviour;
    protected MovementBehaviour _movementBehaviour;
    protected HealthBehaviour _healthBehaviour;

    protected virtual void Awake()
    {
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
        _healthBehaviour = GetComponent<HealthBehaviour>();
    }
}
