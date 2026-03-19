using System.Linq;

public abstract class AInteractable : AProximityPrompt
{
  public override string PromptTag => "InteractPrompt";

  new public void Start()
  {
    base.Start();
  }

  override public void OnPromptInput()
  {
    OnInteract();
  }

  abstract public void OnInteract();
}
