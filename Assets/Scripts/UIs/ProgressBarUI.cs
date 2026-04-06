using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    public Slider progressBar;
    private const int totalProgress = 4;

    void Start()
    {
        if (progressBar)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = 1;
            progressBar.value = 0;
            progressBar.interactable = false;
        }
    }

    void OnEnable()
    {
        GameEvents.OnCheckpointReached += UpdateProgressBar;
    }

    void OnDisable()
    {
        GameEvents.OnCheckpointReached -= UpdateProgressBar;
    }

    void UpdateProgressBar(int index)
    {
        if (progressBar)
        {
            progressBar.value = (float) index / totalProgress;
        }
    }
}
