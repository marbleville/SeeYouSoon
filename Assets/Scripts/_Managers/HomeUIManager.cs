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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }

        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);

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

        if (instructionsDialogueManager != null)
        {
            instructionsDialogueManager.StartDialogue(
                instructionsDialogueManager.introSpeakerName,
                instructionsDialogueManager.introLines
            );
        }
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(true);

        if (instructionsDialogueManager != null)
        {
            instructionsDialogueManager.EndDialogue();
        }
    }
}