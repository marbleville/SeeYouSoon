using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;

    private void Awake()
    {
        if (sensitivitySlider == null)
        {
            sensitivitySlider = GetComponent<Slider>();
        }

        if (sensitivitySlider == null)
        {
            sensitivitySlider = GetComponentInChildren<Slider>(true);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    private void OnEnable()
    {
        RefreshFromSavedValue();
    }

    private void OnDestroy()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
        }
    }

    public void RefreshFromSavedValue()
    {
        float savedValue = GameManager.GetSavedMouseSensitivity();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(savedValue);
        }

        UpdateValueText(savedValue);
    }

    public void OnSensitivityChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMouseSensitivity(value);
        }
        else
        {
            // Fallback for scenes where GameManager is not present yet.
            PlayerPrefs.SetFloat("Settings_MouseSensitivity", value);
            PlayerPrefs.Save();
        }

        UpdateValueText(value);
    }

    private void UpdateValueText(float value)
    {
        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = Mathf.RoundToInt(value).ToString();
        }
    }
}
