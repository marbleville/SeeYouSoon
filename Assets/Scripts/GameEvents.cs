using System;
using System.Net.NetworkInformation;

public static class GameEvents
{
    public static event Action OnDocumentPickedUp;
    public static event Action OnDocumentPutDown;
    public static event Action OnRiddleOneInteracted;
    public static event Action OnRiddleTwoInteracted;
    public static event Action OnRiddleThreeInteracted;
    public static event Action OnRiddleFourInteracted;
    public static event Action OnCafeTableInteracted;
    public static event Action<int> OnCheckpointReached;
    public static event Action OnElevatorInteracted;
 
    public static void TriggerDocumentsPickedUp() {
        OnDocumentPickedUp?.Invoke();
    }

    public static void TriggerDocumentsPutDown() {
        OnDocumentPutDown?.Invoke();
    }

    public static void TriggerRiddleOneInteracted() {
        OnRiddleOneInteracted?.Invoke();
    }

    public static void TriggerRiddleTwoInteracted() {
        OnRiddleTwoInteracted?.Invoke();
    }

    public static void TriggerRiddleThreeInteracted() {
        OnRiddleThreeInteracted?.Invoke();
    }

    public static void TriggerRiddleFourInteracted() {
        OnRiddleFourInteracted?.Invoke();
    }

    public static void TriggerCafeTableSit()
    {
        OnCafeTableInteracted?.Invoke();
    }

    public static void TriggerCheckpointReached(int i) {
        OnCheckpointReached?.Invoke(i);
    }

    public static void TriggerElevatorInteracted()
    {
        OnElevatorInteracted?.Invoke();
    }
}