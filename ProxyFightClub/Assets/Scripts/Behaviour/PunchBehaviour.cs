using System;
using UnityEngine;

public class PunchBehaviour : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private bool _active;

    private void OnTriggerEnter(Collider other)
    {
        if (!_active) return;
        
        if (other.TryGetComponent<HealthBehaviour>(out var health))
        {
            health.TakeDamage(_damage);
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
}