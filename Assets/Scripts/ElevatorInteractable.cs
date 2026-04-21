using UnityEngine;

public class ElevatorInteractable : AInteractable
{
    public ElevatorController elevatorController;
    new void Start()
    {
        base.Start();
    }

  // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void OnInteract()
    {
        if (elevatorController)
        {
            elevatorController.PressOpen();
            GameEvents.TriggerElevatorInteracted();
        }
    }
}
