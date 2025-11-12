using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [SerializeField] private CinemachineInputAxisController _cinemachineInput;

    [Header("EndScreen")]
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private TextMeshProUGUI _gameOverLabel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private bool _isInMenu = false;

    public bool IsFightActive { get; private set; }

    public bool IsInMenu
    {
        get => _isInMenu;
        set => _isInMenu = value;
    }

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

        if (_menuScreen != null)
        {
            _menuScreen.SetActive(false);
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
        if (IsFightActive || _isInMenu) return;

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
        ShowMenu("Defeat");

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnemyDeath()
    {
        if (CurrentEnemy != null && CurrentEnemy.IsBoss)
        {
            ShowMenu("Victory");
        }

        EndFight(true);
    }

    public void ShowMenu(string text)
    {
        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (_menuScreen == null || _gameOverLabel == null) return;

        _gameOverLabel.text = text;
        _menuScreen.SetActive(true);
        _isInMenu = true;
    }

    public void HideMenu()
    {
        if (_menuScreen == null) return;

        _menuScreen.SetActive(false);
        _isInMenu = false;

        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnRestart()
    {
        if (_cinemachineInput != null)
        {
            _cinemachineInput.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
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
}
