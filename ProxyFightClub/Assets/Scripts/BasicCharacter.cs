using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected AttackBehaviour _attackBehaviour;
    protected MovementBehaviour _movementBehaviour;

    protected bool _hasEnemy;

    protected virtual void Awake()
    {
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
    }
}
