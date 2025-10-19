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

        Animator armAnimator;

        if (isLeft)
        {
            armAnimator = _leftArmAnimator;
        }
        else
        {
            armAnimator = _rightArmAnimator;
        }

        armAnimator.SetTrigger(PUNCH_TRIGGER);
    }
}
