using System;
using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Door Animators")]
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;

    [Header("Timing")]
    public float sceneLoadDelay = 2f;

    [Header("Next Scene")]
    public String nextSceneName;

    private bool openPressed = false;

    public void PressOpen()
    {
        if (openPressed) return;
        openPressed = true;

        if (leftDoorAnimator) leftDoorAnimator.SetBool("isOpen", true);
        if (rightDoorAnimator) rightDoorAnimator.SetBool("isOpen", true);

        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(sceneLoadDelay);

        if (GameManager.Instance)
            GameManager.Instance.LoadNextLevel(nextSceneName);
    }
}
