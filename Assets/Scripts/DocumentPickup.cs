using UnityEngine;

public class DocumentPickup : APickupable
{
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    
    public override void OnPickup()
    {
        GameEvents.TriggerDocumentsPickedUp();
    }

    public override void OnDrop()
    {
        GameEvents.TriggerDocumentsPutDown();
    }
}