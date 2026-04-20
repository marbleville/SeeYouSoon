using TMPro;
using UnityEngine;

public class ChooseDialogue : MonoBehaviour
{
    [System.Serializable]
    public class ChoiceStep
    {
        [TextArea(2, 4)]
        public string leftOptionText;

        [TextArea(2, 4)]
        public string rightOptionText;

        [TextArea(2, 5)]
        public string[] leftResponseLines;

        [TextArea(2, 5)]
        public string[] rightResponseLines;
    }

    [Header("References")]
    public DialogueManager dialogueManager;
    public LevelManager levelManager;
    public GameObject leftBox;
    public GameObject rightBox;
    public TMP_Text leftOptionText;
    public TMP_Text rightOptionText;

    [Header("Dialogue")]
    public string admirerSpeakerName = "Admirer";
    public ChoiceStep[] steps;

    [Header("Start Trigger")]
    public bool startOnCafeTableInteract = true;

    private int currentStepIndex = -1;
    private bool isFlowActive = false;
    private bool waitingForChoice = false;
    private bool waitingForResponseToFinish = false;
    
    public bool IsFlowActive => isFlowActive;

    void Awake()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindFirstObjectByType<DialogueManager>();
        }

        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }

        AutoWireOptionTexts();
    }

    void OnEnable()
    {
        if (startOnCafeTableInteract)
        {
            GameEvents.OnCafeTableInteracted += StartFlow;
        }
    }

    void OnDisable()
    {
        if (startOnCafeTableInteract)
        {
            GameEvents.OnCafeTableInteracted -= StartFlow;
        }
    }

    void Start()
    {
        SetChoiceBoxesVisible(false);
    }

    void Update()
    {
        if (!isFlowActive)
            return;

        if (waitingForChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                ChooseLeft();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ChooseRight();
            }

            return;
        }

        if (waitingForResponseToFinish)
        {
            if (dialogueManager == null || !dialogueManager.IsDialogueActive())
            {
                NextStep();
            }
        }
    }

    public void StartFlow()
    {
        if (isFlowActive)
            return;

        if (steps == null || steps.Length == 0)
        {
            Debug.LogWarning("ChooseDialogue has no steps configured.", this);
            return;
        }

        if (dialogueManager != null && dialogueManager.IsDialogueActive())
        {
            dialogueManager.EndDialogue();
        }

        if (dialogueManager != null)
        {
            dialogueManager.enableArrowInput = true;
            dialogueManager.loopDialogue = false;
            dialogueManager.alwaysShowNextArrowWhenActive = true;
        }

        isFlowActive = true;
        currentStepIndex = 0;
        ShowCurrentStep();
    }

    public void BeginSequence()
    {
        StartFlow();
    }

    public void SelectLeftOption()
    {
        ChooseLeft();
    }

    public void SelectRightOption()
    {
        ChooseRight();
    }

    public void ChooseLeft()
    {
        Choose(true);
    }

    public void ChooseRight()
    {
        Choose(false);
    }

    public void NextStep()
    {
        waitingForResponseToFinish = false;
        currentStepIndex++;

        if (currentStepIndex >= steps.Length)
        {
            EndFlow();
            return;
        }

        ShowCurrentStep();
    }

    private void Choose(bool choseLeft)
    {
        if (!isFlowActive || !waitingForChoice)
            return;

        ChoiceStep step = steps[currentStepIndex];
        string[] responseLines = choseLeft ? step.leftResponseLines : step.rightResponseLines;

        waitingForChoice = false;
        SetChoiceBoxesVisible(false);

        if (responseLines == null || responseLines.Length == 0)
        {
            Debug.LogWarning(
                choseLeft
                    ? "ChooseDialogue: Left choice has no response lines. Add lines in Steps -> Left Response Lines."
                    : "ChooseDialogue: Right choice has no response lines. Add lines in Steps -> Right Response Lines.",
                this
            );
            NextStep();
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogWarning("ChooseDialogue is missing DialogueManager.", this);
            NextStep();
            return;
        }

        dialogueManager.StartDialogue(admirerSpeakerName, responseLines);
        waitingForResponseToFinish = true;
    }

    private void ShowCurrentStep()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length)
        {
            EndFlow();
            return;
        }

        ChoiceStep step = steps[currentStepIndex];

        if (leftOptionText != null)
        {
            leftOptionText.text = step.leftOptionText;
        }

        if (rightOptionText != null)
        {
            rightOptionText.text = step.rightOptionText;
        }

        waitingForChoice = true;
        waitingForResponseToFinish = false;
        SetChoiceBoxesVisible(true);
    }

    private void EndFlow()
    {
        isFlowActive = false;
        waitingForChoice = false;
        waitingForResponseToFinish = false;
        SetChoiceBoxesVisible(false);

        if (dialogueManager != null)
        {
            dialogueManager.enableArrowInput = true;
            dialogueManager.alwaysShowNextArrowWhenActive = false;
        }

        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }

        if (levelManager != null)
        {
            levelManager.HandleLevelWin();
        }
    }

    private void SetChoiceBoxesVisible(bool isVisible)
    {
        if (leftBox != null)
        {
            leftBox.SetActive(isVisible);
        }

        if (rightBox != null)
        {
            rightBox.SetActive(isVisible);
        }

        EnsureOptionTextsAreActive();
    }

    private void AutoWireOptionTexts()
    {
        if (leftBox != null && leftOptionText == null)
        {
            TMP_Text[] leftTexts = leftBox.GetComponentsInChildren<TMP_Text>(true);
            if (leftTexts != null && leftTexts.Length > 0)
            {
                leftOptionText = leftTexts[0];
            }
        }

        if (rightBox != null && rightOptionText == null)
        {
            TMP_Text[] rightTexts = rightBox.GetComponentsInChildren<TMP_Text>(true);
            if (rightTexts != null && rightTexts.Length > 0)
            {
                rightOptionText = rightTexts[0];
            }
        }

        EnsureOptionTextsAreActive();
    }

    private void EnsureOptionTextsAreActive()
    {
        if (leftOptionText != null)
        {
            leftOptionText.gameObject.SetActive(true);
        }

        if (rightOptionText != null)
        {
            rightOptionText.gameObject.SetActive(true);
        }
    }

}
