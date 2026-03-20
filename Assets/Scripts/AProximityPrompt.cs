using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AProximityPrompt : MonoBehaviour
{
  public float interactDinstance = 2;
  [Range(0, 1)]
  public float promptFadeBuffer = 0.2f;

  public abstract string PromptTag { get; }

  private GameObject player;
  private float playerDist;
  private static List<AProximityPrompt> instances = new List<AProximityPrompt>();
  private readonly float debounceTime = 0.1f;
  private float debounceTimer = 0;

  public void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");

    if (!player) Debug.Log("No player found.");

    instances.Add(this);
  }

  public void Update()
  {
    debounceTimer -= Mathf.Clamp(debounceTimer - Time.deltaTime, 0, debounceTime);
    playerDist = Vector3.Distance(transform.position, player.transform.position);
    FadePrompt();
    HandlePromptInput();
  }

  private void FadePrompt()
  {
    if (playerDist > interactDinstance || playerDist < (interactDinstance * promptFadeBuffer)) return;

    // If not first, fade to zero 
    float opacity = 0;

    AProximityPrompt[] sortedInstances = instances.OrderBy(i => i.playerDist).ToArray();
    if (this == sortedInstances[0])
    {
      float bufferRatio = (interactDinstance - playerDist) / (interactDinstance * promptFadeBuffer);
      opacity = Mathf.Clamp(bufferRatio, 0f, 1f);
    }

    GameObject interactPromptObject = GameObject.FindGameObjectWithTag(PromptTag);
    CanvasGroup canvasGroup = interactPromptObject.GetComponent<CanvasGroup>();
    canvasGroup.alpha = opacity;
  }

  private void HandlePromptInput()
  {
    if (playerDist > interactDinstance || !Input.GetKeyDown(KeyCode.E) || debounceTimer > 0.05) return;

    debounceTimer = debounceTime;

    AProximityPrompt[] sortedInstances = instances.OrderBy(i => i.playerDist).ToArray();
    AProximityPrompt closestInstance = sortedInstances[0];

    closestInstance.OnPromptInput();
  }

  public abstract void OnPromptInput();
}