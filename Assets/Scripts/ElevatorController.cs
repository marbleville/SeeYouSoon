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

    [Header("Win Flow")]
    [SerializeField] private LevelManager levelManager;

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

        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
        }

        if (levelManager != null)
        {
            levelManager.HandleLevelWin();
            yield break;
        }

        if (GameManager.Instance)
        {
            GameManager.Instance.LoadNextLevel(nextSceneName);
        }
    }
}
