public abstract class ADrivable : AProximityPrompt
{
  public override string PromptTag => "DrivePrompt";

  new public void Start()
  {
    base.Start();
  }

  override public void OnPromptInput()
  {
    OnDrive();
  }

  abstract public void OnDrive();
}
