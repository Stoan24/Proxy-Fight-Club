using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsFightActive { get; private set; }

    private PlayerCharacter _player;
    private EnemyCharacter _currentEnemy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFight(EnemyCharacter enemy)
    {
        if (IsFightActive) return;

        _player = FindFirstObjectByType<PlayerCharacter>();
        _currentEnemy = enemy;
        IsFightActive = true;

        //reset health
        _player.GetComponent<HealthBehaviour>().HealFull();
        _currentEnemy.GetComponent<HealthBehaviour>().HealFull();

        //add deaths
        _player.GetComponent<HealthBehaviour>().OnDeath += OnPlayerDeath;
        _currentEnemy.GetComponent<HealthBehaviour>().OnDeath += OnEnemyDeath;

    }

    private void OnPlayerDeath()
    {
        EndFight(false);
    }

    private void OnEnemyDeath()
    {
        EndFight(true);
    }

    private void EndFight(bool playerWin)
    {
        //remove deaths
        _player.GetComponent<HealthBehaviour>().OnDeath -= OnPlayerDeath;
        _currentEnemy.GetComponent<HealthBehaviour>().OnDeath -= OnEnemyDeath;

        //give statPoints as reward of a win
        if (playerWin && _player != null)
        {
            var stats = _player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                var reward = _currentEnemy.RewardPoint;
                stats.AddStatPoints(reward);
            }
        }

        IsFightActive = false;
        _currentEnemy = null;
    }
}
