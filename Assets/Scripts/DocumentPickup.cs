using UnityEngine;

public class DocumentPickup : APickupable
{
    public override void OnPickup()
    {
        GameEvents.TriggerDocumentsPickedUp();
    }

    public override void OnDrop()
    {
        GameEvents.TriggerDocumentsPutDown();
    }
}