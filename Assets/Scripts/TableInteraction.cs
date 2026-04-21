using UnityEngine;

public class TableInteraction : AInteractable
{
    public override string PromptTag => "InteractPrompt";

    public ParticleSystem tableParticles;

    private bool isSitting = false;
    private Vector3 originalPos;
    private GameObject player;
    private Vector3 sitOffset = new Vector3(0, 0.5f, 0.5f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {
            Debug.LogWarning("No player found in TableInteraction.cs");
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void OnInteract()
    {
        if (!isSitting)
        {
            Sit();
            isActive = false;
            return;
        }

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            return;
        }

    }

    void Sit()
    {
        isSitting = true;

        originalPos = player.transform.position;

        player.transform.SetParent(gameObject.transform);
        player.transform.localPosition = sitOffset;
        player.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180, 0);

        CharacterController controller = player.GetComponent<CharacterController>();
        CapsuleCollider collider = player.GetComponent<CapsuleCollider>();

        controller.enabled = false;
        collider.enabled = false;

        if (tableParticles != null)
        {
            tableParticles.Stop();
        }

        // ChooseDialogue listens to this event and starts the end flow.
        GameEvents.TriggerCafeTableSit();
    }

    void StopSitting()
    {
        isSitting = false;

        player.transform.SetParent(null);
        player.transform.position = originalPos;
    }
}
