using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Timer")]
    [SerializeField] 
    private float totalGameTime = 900f;

    [Header("Scene Names")]
    [SerializeField] 
    private string homeSceneName = "Level0_Home";
    [SerializeField] 
    private string firstLevelSceneName = "Level1_Office";
    [SerializeField] 
    private string secondLevelSceneName = "Level2_City";
    [SerializeField] 
    private string thirdLevelSceneName = "Level3_Cafe";


    public float RemainingTime { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsGameRunning { get; private set; }

    private float levelStartTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        RemainingTime = totalGameTime;
        levelStartTime = totalGameTime;
        IsPaused = false;
        IsGameRunning = false;
    }

    private void Update()
    {
        if (!IsGameRunning || IsPaused)
            return;

        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0f)
        {
            RemainingTime = 0f;
            IsGameRunning = false;
            PauseGame();

            LevelManager currentLevelManager = FindFirstObjectByType<LevelManager>();
            if (currentLevelManager != null)
            {
                currentLevelManager.HandleLevelFail();
            }
        }
    }

    public void StartNewGame()
    {
        RemainingTime = totalGameTime;
        levelStartTime = totalGameTime;
        IsGameRunning = true;
        ResumeGame();
    }

    public void RegisterLevelStart()
    {
        levelStartTime = RemainingTime;
        ResumeGame();
    }

    public void ResetToLevelStartTime()
    {
        RemainingTime = levelStartTime;
        IsGameRunning = true;
        ResumeGame();
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void StopGame()
    {
        IsGameRunning = false;
        PauseGame();
    }

    public void ReturnHome()
    {
        IsGameRunning = false;
        ResumeGame();
        SceneManager.LoadScene(homeSceneName);
    }

    public void RestartFullGame()
    {
        StartNewGame();
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ReloadCurrentLevel()
    {
        ResetToLevelStartTime();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextLevel(string nextSceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(nextSceneName);
    }

    public string FormatTime()
    {
        int minutes = Mathf.FloorToInt(RemainingTime / 60f);
        int seconds = Mathf.FloorToInt(RemainingTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}