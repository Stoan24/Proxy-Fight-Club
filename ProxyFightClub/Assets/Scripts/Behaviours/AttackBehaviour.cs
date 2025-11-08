using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _leftArmAnimator;
    [SerializeField] private Animator _rightArmAnimator;

    [SerializeField] private int _baseDamage = 10;

    private int _currentDamage;

    private const string PUNCH_TRIGGER = "Punch";


    public void Awake()
    {
        _currentDamage = _baseDamage;
    }

    public void Attack(bool isLeft)
    {
        if (_leftArmAnimator == null || _rightArmAnimator == null) return;

        var armAnimator = isLeft ? _leftArmAnimator : _rightArmAnimator;

        armAnimator.SetTrigger(PUNCH_TRIGGER);
    }

    public void SetDamage(int damage)
    {
        _currentDamage = damage;

        var punchBehaviours = GetComponentsInChildren<PunchBehaviour>();

        foreach (var punch in punchBehaviours)
        {
            punch.SetDamage(_currentDamage);
        }
    }
}


