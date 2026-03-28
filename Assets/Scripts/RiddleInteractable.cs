using UnityEngine;

public class RiddleInteractable : AInteractable
{
    public enum RiddleNumber { Riddle1, Riddle2, Riddle3, Riddle4 }
    public RiddleNumber riddleNumber;

    public string speakerName;

    [TextArea(2, 5)]
    public string[] dialogueLines;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    public override void OnInteract()
    {
        if (DialogueManager.Instance && dialogueLines != null)
            DialogueManager.Instance.StartDialogue(speakerName, dialogueLines);

        switch (riddleNumber)
        {
            case RiddleNumber.Riddle1:
                GameEvents.TriggerRiddleOneInteracted();
                GameEvents.TriggerCheckpointReached(1);
                break;
            case RiddleNumber.Riddle2:
                GameEvents.TriggerRiddleTwoInteracted();
                break;
            case RiddleNumber.Riddle3:
                GameEvents.TriggerRiddleThreeInteracted();
                break;
            case RiddleNumber.Riddle4:
                GameEvents.TriggerRiddleFourInteracted();
                GameEvents.TriggerCheckpointReached(2);
                break;
        }
    }
}
