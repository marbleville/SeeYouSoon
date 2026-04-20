using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] 
    private GameObject mainMenuPanel;
    [SerializeField] 
    private GameObject instructionsPanel;
    [SerializeField] 
    private GameObject settingsPanel;
    [SerializeField] 
    private GameObject teamPanel;
    [SerializeField]
    private GameObject continueButton;

    [Header("Instructions Dialogue")]
    [SerializeField] 
    private DialogueManager instructionsDialogueManager;

    [Header("Scene Names")]
    [SerializeField] 
    private string firstLevelSceneName = "Level1_Office";

    private void Start()
    {
        ShowMainMenu();
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(firstLevelSceneName);
            return;
        }

        if (GameManager.Instance.HasSavedProgress())
        {
            GameManager.Instance.ContinueFromSavedProgress();
            return;
        }

        GameManager.Instance.StartNewGame();
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ContinueGame()
    {
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(firstLevelSceneName);
            return;
        }

        if (GameManager.Instance.HasSavedProgress())
        {
            GameManager.Instance.ContinueFromSavedProgress();
        }
        else
        {
            StartGame();
        }
    }

    public void RestartGame()
    {
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(firstLevelSceneName);
            return;
        }

        GameManager.Instance.RestartFullGame();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        teamPanel.SetActive(false);

        if (continueButton != null && GameManager.Instance != null)
        {
            continueButton.SetActive(GameManager.Instance.HasSavedProgress());
        }

        if (instructionsDialogueManager != null)
        {
            instructionsDialogueManager.EndDialogue();
        }
    }

    public void ShowInstructions()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(true);
        settingsPanel.SetActive(false);
        teamPanel.SetActive(false);

        if (instructionsDialogueManager != null)
        {
            instructionsDialogueManager.StartDialogue(
                instructionsDialogueManager.speakerName,
                instructionsDialogueManager.dialogueLines
            );
        }
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(true);
        teamPanel.SetActive(false);

        if (instructionsDialogueManager != null)
        {
            instructionsDialogueManager.EndDialogue();
        }
    }

    public void ShowTeam()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        teamPanel.SetActive(true);
    }
}
