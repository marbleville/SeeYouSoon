using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AProximityPrompt : MonoBehaviour
{
  public float interactDinstance = 2;
  [Range(0, 1)]
  public float promptFadeBuffer = 0.2f;
  public bool isActive = true;

  public abstract string PromptTag { get; }
  protected virtual int InputPriority => 0;

  private GameObject player;
  private float playerDist;
  private static List<AProximityPrompt> instances = new List<AProximityPrompt>();
  private readonly float debounceTime = 0.1f;
  private static float inputDebounceTimer = 0;
  private static int lastConsumedInputFrame = -1;

  public void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");

    if (!player) Debug.Log("No player found.");

    if (!instances.Contains(this))
    {
      instances.Add(this);
    }
  }

  private void OnEnable()
  {
    if (!instances.Contains(this))
    {
      instances.Add(this);
    }
  }

  public void Update()
  {
    // Clamp keeps debounce timer bounded and stable as it counts down each frame.
    inputDebounceTimer = Mathf.Clamp(inputDebounceTimer - Time.deltaTime, -0.1f, debounceTime);

    if (!player) return;

    playerDist = Vector3.Distance(transform.position, player.transform.position);
    FadePrompt();
    HandlePromptInput();
  }

  private void FadePrompt()
  {
    GameObject interactPromptObject = GameObject.FindGameObjectWithTag(PromptTag);
    if (!interactPromptObject) return;

    CanvasGroup canvasGroup = interactPromptObject.GetComponent<CanvasGroup>();
    if (!canvasGroup) return;

    // Avoid multiple instances fighting over the same prompt canvas alpha
    AProximityPrompt closestForTag = GetClosestInstanceForPromptTag();
    if (closestForTag != this) return;

    float opacity = 0;
    if (!ShouldNotShowPrompt())
    {
      float fadeRange = interactDinstance * Mathf.Max(promptFadeBuffer, 0.0001f);
      float startFadeAt = interactDinstance - fadeRange;

      // Full opacity when close; fade only near the outer interaction boundary
      if (playerDist <= startFadeAt)
      {
        opacity = 1f;
      }
      else
      {
        float bufferRatio = (interactDinstance - playerDist) / fadeRange;
        opacity = Mathf.Clamp(bufferRatio, 0f, 1f);
      }
    }

    canvasGroup.alpha = opacity;
  }

  private bool ShouldNotShowPrompt()
  {
    bool isDialogueActive = DialogueManager.Instance && DialogueManager.Instance.IsDialogueActive();
    return playerDist > interactDinstance || inputDebounceTimer > 0 || isDialogueActive || !isActive;
  }

  private void HandlePromptInput()
  {
    if (!Input.GetKeyDown(KeyCode.E)) return;
    if (lastConsumedInputFrame == Time.frameCount) return;

    bool isDialogueActive = DialogueManager.Instance && DialogueManager.Instance.IsDialogueActive();
    if (isDialogueActive || inputDebounceTimer > 0) return;

    AProximityPrompt nextPrompt = GetInputTargetFromVisiblePrompt();
    if (nextPrompt == null) return;

    lastConsumedInputFrame = Time.frameCount;
    inputDebounceTimer = debounceTime;
    nextPrompt.OnPromptInput();
  }

  private AProximityPrompt GetClosestInstanceForPromptTag()
  {
    return instances
      .Where(i =>
        i != null &&
        i.isActiveAndEnabled &&
        i.gameObject.activeInHierarchy &&
        i.player != null &&
        i.PromptTag == PromptTag)
      .OrderBy(i => i.playerDist)
      .FirstOrDefault();
  }

  private AProximityPrompt GetInputTargetFromVisiblePrompt()
  {
    AProximityPrompt heldPickup = instances
      .Where(i =>
        i != null &&
        i.isActiveAndEnabled &&
        i.gameObject.activeInHierarchy &&
        i.player != null &&
        i.playerDist <= i.interactDinstance)
      .OfType<APickupable>()
      .Where(p => p.IsHolding)
      .OrderBy(p => p.playerDist)
      .FirstOrDefault();
    if (heldPickup != null) return heldPickup;

    var visibleTargets = instances
      .Where(i =>
        i != null &&
        i.isActiveAndEnabled &&
        i.gameObject.activeInHierarchy &&
        i.player != null &&
        i.playerDist <= i.interactDinstance)
      .GroupBy(i => i.PromptTag)
      .Select(group =>
      {
        AProximityPrompt closest = group.OrderBy(i => i.playerDist).FirstOrDefault();
        return new
        {
          Closest = closest,
          Alpha = GetPromptAlpha(group.Key)
        };
      })
      .Where(x => x.Closest != null && x.Alpha > 0.01f)
      .OrderByDescending(x => x.Alpha)
      .ThenBy(x => x.Closest.playerDist)
      .Select(x => x.Closest)
      .FirstOrDefault();

    if (visibleTargets != null) return visibleTargets;

    return instances
      .Where(i =>
        i != null &&
        i.isActiveAndEnabled &&
        i.gameObject.activeInHierarchy &&
        i.player != null &&
        i.playerDist <= i.interactDinstance)
      .OrderByDescending(i => i.InputPriority)
      .ThenBy(i => i.playerDist)
      .FirstOrDefault();
  }

  private float GetPromptAlpha(string promptTag)
  {
    GameObject promptObject = GameObject.FindGameObjectWithTag(promptTag);
    if (!promptObject) return 0f;

    CanvasGroup canvasGroup = promptObject.GetComponent<CanvasGroup>();
    if (!canvasGroup) return 0f;

    return canvasGroup.alpha;
  }

  private void OnDisable()
  {
    instances.Remove(this);
  }

  private void OnDestroy()
  {
    instances.Remove(this);
  }

  public abstract void OnPromptInput();
}
