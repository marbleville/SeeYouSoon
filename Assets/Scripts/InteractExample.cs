using UnityEngine;

public class InteractExample : AInteractable
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    Debug.Log("Interacted!");
  }
}
