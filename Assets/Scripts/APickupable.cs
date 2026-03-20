using NUnit.Framework;
using UnityEngine;

public abstract class APickupable : AProximityPrompt
{
  public override string PromptTag { get; } = "PickupPrompt";

  private Vector3 pickupOffset = new Vector3(1, 0.5f, 0);
  private bool isHolding = false;

  new public void Start()
  {
    base.Start();
  }

  override public void OnPromptInput()
  {
    if (!isHolding)
    {
      PickupObject();
    }
    else
    {
      DropObject();
    }

    isHolding = !isHolding;
  }

  private void PickupObject()
  {
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (!player)
    {
      Debug.Log("No player found.");
      return;
    }

    gameObject.transform.SetParent(player.transform);
    gameObject.transform.localPosition = pickupOffset;

    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
    if (rb) { rb.isKinematic = true; }

    OnPickup();
  }

  private void DropObject()
  {
    gameObject.transform.SetParent(null);

    Rigidbody rb = gameObject.GetComponent<Rigidbody>();
    if (rb) { rb.isKinematic = false; }

    OnDrop();
  }


  public abstract void OnPickup();

  public abstract void OnDrop();
}