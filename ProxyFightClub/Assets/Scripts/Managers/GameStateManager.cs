using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("EndScreen")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private TextMeshProUGUI _gameOverLabel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;


    public bool IsFightActive { get; private set; }
    public bool IsInMenu { get; private set; }

    private PlayerCharacter _player;

    private EnemyHealthBar _enemyHealthBar;

    public EnemyCharacter CurrentEnemy { get; private set; }

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

        if (_gameOverScreen != null)
        {
            _gameOverScreen.SetActive(false);
        }

        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(OnRestart);
        }

        if (_exitButton != null)
        {
            _exitButton.onClick.AddListener(OnExit);
        }
    }

    private void Start()
    {
        _enemyHealthBar = FindAnyObjectByType<EnemyHealthBar>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region FightManagement

    public void StartFight(EnemyCharacter enemy)
    {
        if (IsFightActive || IsInMenu) return;

        _player = FindFirstObjectByType<PlayerCharacter>();
        CurrentEnemy = enemy;
        IsFightActive = true;

        //reset health
        _player.GetComponent<HealthBehaviour>().HealFull();
        CurrentEnemy.GetComponent<HealthBehaviour>().HealFull();

        //add deaths
        _player.GetComponent<HealthBehaviour>().OnDeath += OnPlayerDeath;
        CurrentEnemy.GetComponent<HealthBehaviour>().OnDeath += OnEnemyDeath;

        if (_enemyHealthBar != null)
        {
            _enemyHealthBar.ShowBar(enemy);
        }

        InteractionMenu.Instance?.Hide();
    }

    private void EndFight(bool playerWin)
    {
        if (_player == null || CurrentEnemy == null) return;

        //remove deaths
        var playerHealth = _player.GetComponent<HealthBehaviour>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= OnPlayerDeath;
            playerHealth.HealFull();
        }

        var enemyHealth = _player.GetComponent<HealthBehaviour>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= OnEnemyDeath;
        }

        //give statPoints as reward of a win
        if (playerWin && _player != null)
        {
            var stats = _player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                var reward = CurrentEnemy.RewardPoint;
                stats.AddStatPoints(reward);
            }
        }

        if (_enemyHealthBar != null)
        {
            _enemyHealthBar.HideBar();
        }

        IsFightActive = false;
        CurrentEnemy = null;
    }
    #endregion

    #region GameOverManagement
    private void OnPlayerDeath()
    {
        EndFight(false);
        ShowGameOver("Defeat");
    }

    private void OnEnemyDeath()
    {
        EndFight(true);

        if (CurrentEnemy != null && CurrentEnemy.IsBoss)
        {
            ShowGameOver("Victory");
        }

        CurrentEnemy = null;
    }

    private void ShowGameOver(string text)
    {
        if (_gameOverScreen == null || _gameOverLabel == null) return;

        _gameOverLabel.text = text;
        _gameOverScreen.SetActive(true);
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #endregion

    public void MenuLock(bool inMenu)
    {
        IsInMenu = inMenu;

        var inputProvider = FindAnyObjectByType<CinemachineInputAxisController>();
        if (inputProvider != null)
        {
            inputProvider.enabled = !inMenu;
        }

        Cursor.visible = inMenu;
        Cursor.lockState = inMenu ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
