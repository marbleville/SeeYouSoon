using UnityEngine;
using TMPro;
using System.Collections;

public class TimedDialogue : MonoBehaviour
{
    [System.Serializable]
    private class TimedLine
    {
        [TextArea(2, 5)]
        public string text;
        public float showFor = 2.5f;
        public float delayFor = 0.75f;
        public Collider triggerZone;
    }

    [Header("Dialogue")]
    [SerializeField] private string speakerName = "Player";
    [SerializeField] private TimedLine[] lines;

    [Header("Timing")]
    [SerializeField] private float startDelay = 0.75f;
    [SerializeField] private bool loop = false;
    [SerializeField] private float loopDelay = 2f;

    [Header("Drive Trigger")]
    [SerializeField] private Prometheus targetCar;

    [Header("Shared Dialogue Canvas")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Conflict Handling")]
    [SerializeField] private bool stopRegularDialogueWhenStarting = true;

    private Coroutine timedDialogueRoutine;
    private bool isShowingTimedDialogue;
    private bool wasDriving;
    private Collider[] targetCarColliders;

    void Awake()
    {
        ResolveCarReference();
        CacheCarColliders();
        ResolveUIReferences();
        HideTimedDialogue();
    }

    void Update()
    {
        if (targetCar == null) return;

        bool isDriving = targetCar.IsDriven;

        if (isDriving && !wasDriving)
        {
            StartTimedDialogue();
        }

        if (!isDriving && wasDriving)
        {
            StopTimedDialogue();
        }

        wasDriving = isDriving;
    }

    void OnDisable()
    {
        StopTimedDialogue();
    }

    public void StartTimedDialogue()
    {
        ResolveUIReferences();
        CacheCarColliders();
        if (!CanPlayTimedDialogue()) return;

        if (stopRegularDialogueWhenStarting && dialogueManager != null && dialogueManager.IsDialogueActive())
        {
            dialogueManager.EndDialogue();
        }

        if (timedDialogueRoutine != null)
        {
            StopCoroutine(timedDialogueRoutine);
        }

        HideTimedDialogue();
        timedDialogueRoutine = StartCoroutine(RunTimedDialogue(true));
    }

    public void StopTimedDialogue()
    {
        if (timedDialogueRoutine != null)
        {
            StopCoroutine(timedDialogueRoutine);
            timedDialogueRoutine = null;
        }

        HideTimedDialogue();
    }

    private IEnumerator RunTimedDialogue(bool shouldApplyStartDelay)
    {
        // Clamp timing values so negative Inspector values never break the sequence
        float safeStartDelay = Mathf.Max(0f, startDelay);
        float safeLoopDelay = Mathf.Max(0f, loopDelay);

        if (shouldApplyStartDelay && safeStartDelay > 0f)
        {
            // Coroutine pause before showing the first timed line
            yield return new WaitForSeconds(safeStartDelay);
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].triggerZone != null)
            {
                yield return StartCoroutine(WaitForTriggerZone(lines[i].triggerZone));
            }

            ShowTimedDialogue();
            dialogueText.text = lines[i].text;
            // Keep each line visible for at least a tiny amount of time
            float showTimeForThisLine = Mathf.Max(0.05f, lines[i].showFor);
            // Delay can be zero, but never negative
            float delayForThisLine = Mathf.Max(0f, lines[i].delayFor);
            bool isLastLine = i == lines.Length - 1;

            // Coroutine pause while the current line is on screen
            yield return new WaitForSeconds(showTimeForThisLine);

            HideTimedDialogue();

            if (!isLastLine && delayForThisLine > 0f)
            {
                yield return new WaitForSeconds(delayForThisLine);
            }
        }

        if (loop)
        {
            if (safeLoopDelay > 0f)
            {
                // Coroutine pause between full dialogue loops.
                yield return new WaitForSeconds(safeLoopDelay);
            }

            timedDialogueRoutine = StartCoroutine(RunTimedDialogue(false));
            yield break;
        }

        timedDialogueRoutine = null;
    }

    private IEnumerator WaitForTriggerZone(Collider triggerZone)
    {
        if (triggerZone == null)
        {
            yield break;
        }

        // Coroutine waits frame-by-frame until the car overlaps this trigger zone.
        while (!IsCarInTriggerZone(triggerZone))
        {
            yield return null;
        }
    }

    private bool IsCarInTriggerZone(Collider triggerZone)
    {
        if (triggerZone == null || targetCarColliders == null || targetCarColliders.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < targetCarColliders.Length; i++)
        {
            Collider carCollider = targetCarColliders[i];

            if (carCollider == null || !carCollider.enabled)
            {
                continue;
            }

            if (triggerZone.bounds.Intersects(carCollider.bounds))
            {
                return true;
            }
        }

        return false;
    }

    private void ShowTimedDialogue()
    {
        isShowingTimedDialogue = true;

        dialogueBox.SetActive(true);
        speakerNameText.text = speakerName;
    }

    private void HideTimedDialogue()
    {
        isShowingTimedDialogue = false;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    private void ResolveUIReferences()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindFirstObjectByType<DialogueManager>();
        }

        if (dialogueManager == null) return;

        if (dialogueBox == null) dialogueBox = dialogueManager.dialogueBox;
        if (speakerNameText == null) speakerNameText = dialogueManager.speakerNameText;
        if (dialogueText == null) dialogueText = dialogueManager.dialogueText;
    }

    private void ResolveCarReference()
    {
        if (targetCar == null)
        {
            targetCar = FindFirstObjectByType<Prometheus>();
        }
    }

    private void CacheCarColliders()
    {
        if (targetCar == null)
        {
            targetCarColliders = null;
            return;
        }

        targetCarColliders = targetCar.GetComponentsInChildren<Collider>();
    }

    private bool CanPlayTimedDialogue()
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("TimedDialogue has no lines assigned", this);
            return false;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i].text))
            {
                Debug.LogWarning("TimedDialogue has an empty line text", this);
                return false;
            }
        }

        if (targetCar == null)
        {
            Debug.LogWarning("TimedDialogue is missing target car reference", this);
            return false;
        }

        if (targetCarColliders == null || targetCarColliders.Length == 0)
        {
            CacheCarColliders();
        }

        if (dialogueBox == null || speakerNameText == null || dialogueText == null)
        {
            Debug.LogWarning(
                "TimedDialogue is missing dialogue UI references",
                this);
            return false;
        }

        return true;
    }
}
