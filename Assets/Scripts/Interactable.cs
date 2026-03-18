using UnityEngine;

public abstract class AInteractable : MonoBehaviour
{
  public float interactDinstance = 10;
  [Range(0, 1)]
  public float promptFadeBuffer = 0.2f;

  private GameObject player;
  private float playerDist;
  private string promptTag = "InteractPrompt";

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");

    if (!player) Debug.Log("No player found.");
  }

  void Update()
  {
    playerDist = Vector3.Distance(transform.position, player.transform.position);
    FadePrompt();
    HandleInteract();
  }

  private void FadePrompt()
  {
    float bufferRatio = (interactDinstance - playerDist) / (interactDinstance * promptFadeBuffer);
    float opacity = Mathf.Clamp(bufferRatio, 0f, 1f);

    GameObject interactPromptObject = GameObject.FindGameObjectWithTag(promptTag);
    CanvasGroup canvasGroup = interactPromptObject.GetComponent<CanvasGroup>();
    canvasGroup.alpha = opacity;
  }

  private void HandleInteract()
  {
    if (playerDist > interactDinstance || !Input.GetKeyDown(KeyCode.E)) return;

    OnInteract();
  }

  public abstract void OnInteract();
}