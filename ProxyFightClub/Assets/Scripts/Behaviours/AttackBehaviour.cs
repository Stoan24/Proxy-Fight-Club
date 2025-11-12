using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _leftArmAnimator;
    [SerializeField] private Animator _rightArmAnimator;

    [SerializeField] private int _baseDamage = 10;
    [SerializeField] private float _staminaCost = 10f;

    private StaminaBehaviour _stamina;

    private UpgradeMenu _upgradeMenu;

    private int _currentDamage;

    private const string PUNCH_TRIGGER = "Punch";


    public void Awake()
    {
        _currentDamage = _baseDamage;

        _stamina = GetComponent<StaminaBehaviour>();
        _upgradeMenu = FindAnyObjectByType<UpgradeMenu>();
    }

    public void Attack(bool isLeft)
    {
        if (_leftArmAnimator == null || _rightArmAnimator == null) return;

        if (_upgradeMenu != null && _upgradeMenu.IsActive) return;
        
        if (isLeft)
        {
            if (!_leftArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("LeftPunch"))
            {
                if (_stamina != null && !_stamina.SpendStamina(_staminaCost)) return;
                _leftArmAnimator.SetTrigger(PUNCH_TRIGGER);
            }
        }
        else
        {
            if (!_rightArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("LeftPunch"))
            {
                if (_stamina != null && !_stamina.SpendStamina(_staminaCost)) return;
                _rightArmAnimator.SetTrigger(PUNCH_TRIGGER);
            }
        }
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


