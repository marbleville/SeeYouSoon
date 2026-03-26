using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private bool isFinalLevel = false;
    [SerializeField] private string nextSceneName = "";

    [Header("Panels")]
    [SerializeField] 
    private GameObject pausePanel;
    [SerializeField] 
    private GameObject pauseSettingsPanel;
    [SerializeField] 
    private GameObject failPanel;
    [SerializeField] 
    private GameObject winPanel;
    [SerializeField] 
    private GameObject finalWinPanel;

    [Header("Player Control Scripts")]
    [SerializeField] 
    private GameObject gameplayScriptsRoot;

    private bool levelEnded = false;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterLevelStart();
        }

        HideAllMenus();
        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);
    }

    private void Update()
    {
        // Will remove at end - for testing purposes
        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleLevelFail();
        }

        // Will remove at end - for testing purposes
        if (Input.GetKeyDown(KeyCode.K))
        {
            HandleLevelWin();
        }

        if (levelEnded)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseSettingsPanel != null && !pauseSettingsPanel.activeSelf)
            {
                PauseLevel();
            }
            else if (pausePanel != null && pausePanel.activeSelf)
            {
                ResumeLevel();
            }
        }
    }

    private void HideAllMenus()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseSettingsPanel != null) pauseSettingsPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (finalWinPanel != null) finalWinPanel.SetActive(false);
    }

    private void UnlockCursorForMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursorForGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetGameplayScriptsEnabled(bool enabled)
    {
        if (gameplayScriptsRoot == null) return;

        MonoBehaviour[] scripts = gameplayScriptsRoot.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null && script != this)
            {
                script.enabled = enabled;
            }
        }
    }

    public void PauseLevel()
    {
        if (levelEnded)
            return;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (pauseSettingsPanel != null) pauseSettingsPanel.SetActive(false);

        UnlockCursorForMenu();
        SetGameplayScriptsEnabled(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void ResumeLevel()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseSettingsPanel != null) pauseSettingsPanel.SetActive(false);

        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void ShowPauseSettings()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseSettingsPanel != null) pauseSettingsPanel.SetActive(true);

        UnlockCursorForMenu();
        SetGameplayScriptsEnabled(false);
    }

    public void ResetLevel()
    {
        levelEnded = false;
        HideAllMenus();
        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReloadCurrentLevel();
        }
    }

    public void ReturnHome()
    {
        levelEnded = false;
        HideAllMenus();
        UnlockCursorForMenu();
        SetGameplayScriptsEnabled(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnHome();
        }
    }

    public void HandleLevelFail()
    {
        levelEnded = true;
        HideAllMenus();

        if (failPanel != null) failPanel.SetActive(true);

        UnlockCursorForMenu();
        SetGameplayScriptsEnabled(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void HandleLevelWin()
    {
        levelEnded = true;
        HideAllMenus();

        UnlockCursorForMenu();
        SetGameplayScriptsEnabled(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }

        if (isFinalLevel)
        {
            if (finalWinPanel != null) finalWinPanel.SetActive(true);
        }
        else
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        levelEnded = false;
        HideAllMenus();
        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);

        if (GameManager.Instance != null && !string.IsNullOrEmpty(nextSceneName))
        {
            GameManager.Instance.LoadNextLevel(nextSceneName);
        }
    }

    public void PlayAgain()
    {
        levelEnded = false;
        HideAllMenus();
        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartFullGame();
        }
    }
}