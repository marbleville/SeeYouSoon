using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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
    public float MouseSensitivity { get; private set; }

    private float levelStartTime;
    private bool suppressPauseAutoSave = false;

    private const string SaveExistsKey = "Save_Exists";
    private const string SaveSceneNameKey = "Save_SceneName";
    private const string SaveRemainingTimeKey = "Save_RemainingTime";
    private const string SaveLevelStartTimeKey = "Save_LevelStartTime";
    private const string MouseSensitivityKey = "Settings_MouseSensitivity";
    private const float DefaultMouseSensitivity = 200f;
    private const float MinMouseSensitivity = 20f;
    private const float MaxMouseSensitivity = 800f;


    // Broadcasts new sensitivity value to listeners 
    // whenever settings UI updates so active scenes apply changes
    public static event Action<float> OnMouseSensitivityChanged;

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
        MouseSensitivity = GetSavedMouseSensitivity();
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
        ClearSavedProgress();
        ResumeGame();
    }

    public void RegisterLevelStart()
    {
        levelStartTime = RemainingTime;
        IsGameRunning = true;
        ResumeGame();
        SaveProgress();
    }

    public void ResetToLevelStartTime()
    {
        RemainingTime = levelStartTime;
        IsGameRunning = true;
        ResumeGame();
        SaveProgress();
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        if (!suppressPauseAutoSave)
        {
            SaveProgress();
        }
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
        SaveProgress();
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
        IsGameRunning = true;
        ResumeGame();
        SaveProgress(nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    public bool HasSavedProgress()
    {
        return PlayerPrefs.GetInt(SaveExistsKey, 0) == 1;
    }

    public void ContinueFromSavedProgress()
    {
        if (!HasSavedProgress())
            return;

        RemainingTime = Mathf.Clamp(PlayerPrefs.GetFloat(SaveRemainingTimeKey, totalGameTime), 0f, totalGameTime);
        levelStartTime = Mathf.Clamp(PlayerPrefs.GetFloat(SaveLevelStartTimeKey, RemainingTime), 0f, totalGameTime);
        IsGameRunning = true;
        ResumeGame();

        string savedSceneName = PlayerPrefs.GetString(SaveSceneNameKey, firstLevelSceneName);
        SceneManager.LoadScene(savedSceneName);
    }

    public void ClearSavedProgress()
    {
        PlayerPrefs.DeleteKey(SaveExistsKey);
        PlayerPrefs.DeleteKey(SaveSceneNameKey);
        PlayerPrefs.DeleteKey(SaveRemainingTimeKey);
        PlayerPrefs.DeleteKey(SaveLevelStartTimeKey);
        PlayerPrefs.Save();
    }

    public void SaveProgress(string sceneNameOverride = "")
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = string.IsNullOrEmpty(sceneNameOverride) ? currentScene.name : sceneNameOverride;

        // Home scene does not represent in-level progress
        if (sceneName == homeSceneName)
            return;

        PlayerPrefs.SetInt(SaveExistsKey, 1);
        PlayerPrefs.SetString(SaveSceneNameKey, sceneName);
        PlayerPrefs.SetFloat(SaveRemainingTimeKey, RemainingTime);
        PlayerPrefs.SetFloat(SaveLevelStartTimeKey, levelStartTime);
        PlayerPrefs.Save();
    }

    public void PauseGameWithoutSaving()
    {
        suppressPauseAutoSave = true;
        PauseGame();
        suppressPauseAutoSave = false;
    }

    public string FormatTime()
    {
        int minutes = Mathf.FloorToInt(RemainingTime / 60f);
        int seconds = Mathf.FloorToInt(RemainingTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public static float GetSavedMouseSensitivity()
    {
        return Mathf.Clamp(
            PlayerPrefs.GetFloat(MouseSensitivityKey, DefaultMouseSensitivity),
            MinMouseSensitivity,
            MaxMouseSensitivity
        );
    }

    public void SetMouseSensitivity(float value)
    {
        float clamped = Mathf.Clamp(value, MinMouseSensitivity, MaxMouseSensitivity);
        MouseSensitivity = clamped;
        PlayerPrefs.SetFloat(MouseSensitivityKey, clamped);
        PlayerPrefs.Save();
        OnMouseSensitivityChanged?.Invoke(clamped);
    }
}
