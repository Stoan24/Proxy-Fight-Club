using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _barContainer;

    [SerializeField] private Image _healthBar;

    private HealthBehaviour _enemyHealth;

    private void Start()
    {
        HideBar();
    }

    private void Update()
    {
        Sync();
    }

    private void Sync()
    {
        if (_enemyHealth == null)
        {
            SetHealth(0, 100);
        }
        else
        {
            SetHealth(_enemyHealth.currentHealth, _enemyHealth.maxHealth);
        }
    }

    private void SetHealth(int current, int max)
    {
        if (_healthBar == null) return;
        _healthBar.transform.localScale = new Vector3((float)current / max, 1.0f, 1.0f);
    }

    public void ShowBar(EnemyCharacter enemy)
    {
        if (enemy == null) return;

        _enemyHealth = enemy.GetComponent<HealthBehaviour>();

        if (_barContainer != null)
        {
            _barContainer.SetActive(true);
        }
    }

    public void HideBar()
    {
        if (_barContainer != null)
        {
            _barContainer.SetActive(false);

            _enemyHealth = null;
        }
    }
}
