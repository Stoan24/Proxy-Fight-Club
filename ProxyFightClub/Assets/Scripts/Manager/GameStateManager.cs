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
        _currentEnemy = enemy;
        IsFightActive = true;

        // Position both fighters facing each other
        _player = FindFirstObjectByType<PlayerCharacter>();
        _player.transform.position = new Vector3(0, 0, 0);

        enemy.transform.position = new Vector3(0, 0, 10);
        enemy.transform.LookAt(_player.transform);

        var playerHealth = _player.GetComponent<HealthBehaviour>();
        var enemyHealth = _currentEnemy.GetComponent<HealthBehaviour>();

        playerHealth.OnDeath += EndFight;
        enemyHealth.OnDeath += EndFight;
    }

    private void EndFight()
    {
        IsFightActive = false;
        _currentEnemy = null;
    }
}
