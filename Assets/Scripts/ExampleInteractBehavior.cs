using UnityEngine;

public class ExampleInteractBehavior : AInteractable
{
    override public void OnInteract()
    {
        Debug.Log("Interacted!");
    }
}
