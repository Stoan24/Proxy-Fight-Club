using System;
using UnityEngine;

public class PunchBehaviour : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private bool _active;

    private BasicCharacter _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<BasicCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_active) return;

        if (other.transform == transform || other.transform.IsChildOf(_owner.transform)) return;

        
        if (other.TryGetComponent<HealthBehaviour>(out var health))
        {
            health.TakeDamage(_damage);

            //deactivate after hit so it doesn't hit twice in one punch
            DeActivate();
        }
    }

    public void Activate()
    {
        _active = true;
    }

    public void DeActivate()
    {
        _active = false;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}