using UnityEngine;

public class ElevatorInteractable : AInteractable
{
    public override void OnInteract()
    {
        if (ElevatorController.Instance)
        {
            ElevatorController.Instance.TriggerElevator();
        }
    }
}
