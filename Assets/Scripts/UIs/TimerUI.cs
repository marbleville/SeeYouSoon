using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text timerText;

    void Update()
    {
        if (GameManager.Instance == null || timerText == null)
            return;

        timerText.text = GameManager.Instance.FormatTime();
    }
}