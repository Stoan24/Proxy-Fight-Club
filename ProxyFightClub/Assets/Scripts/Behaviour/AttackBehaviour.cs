using UnityEngine;
using UnityEngine.InputSystem;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _leftArmAnimator;
    [SerializeField] private Animator _rightArmAnimator;

    private const string PUNCH_TRIGGER = "Punch";

    public void Attack(bool isLeft)
    {
        if (_leftArmAnimator == null || _rightArmAnimator == null) return;

        var armAnimator = isLeft ? _leftArmAnimator : _rightArmAnimator;

        armAnimator.SetTrigger(PUNCH_TRIGGER);
    }
}
