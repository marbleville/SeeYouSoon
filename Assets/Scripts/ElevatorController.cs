using System;
using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController Instance;
    public Animator doorAnimator;

    [Header("Timing")]
    public int closeDelay = 2;
    public int sceneLoadDelay = 1;

    [Header("Next Scene")]
    public String nextSceneName;

    private bool isTriggered = false;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerElevator()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(ElevatorSequence());
    }

    IEnumerator ElevatorSequence()
    {
        if (doorAnimator) 
            doorAnimator.SetTrigger("Open");
        
        yield return new WaitForSeconds(closeDelay);

        if (doorAnimator)
            doorAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(sceneLoadDelay);

        if (GameManager.Instance)
            GameManager.Instance.LoadNextLevel(nextSceneName);
    }
}
