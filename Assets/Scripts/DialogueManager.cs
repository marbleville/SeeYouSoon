using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialogueBox;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public GameObject nextArrow;
    public GameObject previousArrow;

    [Header("Dialogue Instructions")]
    public string speakerName = "Guide";

    [TextArea(2, 5)]
    public string[] dialogueLines; 

    [Header("Options")]
    public bool playIntroOnStart = false;

    private string currentSpeaker;
    private string[] currentLines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    void Awake()
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

    void Start()
    {
        dialogueBox.SetActive(false);
        nextArrow.SetActive(false);
        previousArrow.SetActive(false);

        if (playIntroOnStart)
        {
            StartDialogue(speakerName, dialogueLines);
        }
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousLine();
        }
    }

    public void StartDialogue(string speakerName, string[] lines)
    {
        if (lines == null || lines.Length == 0 || IsDialogueActive()) return;

        currentSpeaker = speakerName;
        currentLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;

        dialogueBox.SetActive(true);
        ShowCurrentLine();
        UpdateArrows();
    }

    void ShowCurrentLine()
    {
        speakerNameText.text = currentSpeaker;
        dialogueText.text = currentLines[currentLineIndex];
    }

    void UpdateArrows()
    {
        if (previousArrow != null)
            previousArrow.SetActive(currentLineIndex > 0);

        if (nextArrow != null)
            nextArrow.SetActive(currentLineIndex < currentLines.Length - 1);
    }

    public void NextLine()
    {
        if (!isDialogueActive) return;

        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            currentLineIndex = 0;
        }

        ShowCurrentLine();
        UpdateArrows();
    }

    public void PreviousLine()
    {
        if (!isDialogueActive) return;

        if (currentLineIndex > 0)
        {
            currentLineIndex--;
            ShowCurrentLine();
            UpdateArrows();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";

        if (nextArrow != null)
            nextArrow.SetActive(false);

        if (previousArrow != null)
            previousArrow.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}