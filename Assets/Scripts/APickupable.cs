using System.Linq;

public abstract class APickupable : AProximityPrompt
{
  public override string PromptTag { get; } = "PickupPrompt";

  new public void Start()
  {
    base.Start();
  }

  override public void OnPromptInput()
  {
    OnPickup();
  }


  public abstract void OnPickup();
}