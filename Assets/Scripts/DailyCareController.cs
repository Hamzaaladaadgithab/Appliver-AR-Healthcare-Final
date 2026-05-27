using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyCareController : MonoBehaviour
{
    [Header("Checklist Toggles")]
    public Toggle[] toggles;

    [Header("Progress UI Elements")]
    public TextMeshProUGUI progressText;
    public Image progressFillImage;

    private const string PrefsPrefix = "DailyCare_Task_";

    private void Start()
    {
        // Load saved states from PlayerPrefs if available
        LoadStates();

        // Register listener for each toggle to update the UI on change
        if (toggles != null)
        {
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i] != null)
                {
                    int index = i; // Avoid closure issue
                    toggles[i].onValueChanged.RemoveAllListeners();
                    toggles[i].onValueChanged.AddListener((val) => {
                        SaveState(index, val);
                        UpdateProgressUI();
                    });
                }
            }
        }

        // Initialize UI display
        UpdateProgressUI();
    }

    /// <summary>
    /// Calculates the dynamic progress and updates the text and fill image.
    /// </summary>
    public void UpdateProgressUI()
    {
        if (toggles == null || toggles.Length == 0) return;

        int completedCount = 0;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null && toggles[i].isOn)
            {
                completedCount++;
            }
        }

        float progressPercentage = (float)completedCount / toggles.Length; // 0.0f to 1.0f
        int percentValue = Mathf.RoundToInt(progressPercentage * 100);

        // Update progress text (e.g. "Günlük Uyum: %60")
        if (progressText != null)
        {
            progressText.text = "Günlük Uyum: %" + percentValue;
        }

        // Update progress fill bar dynamically
        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = progressPercentage;
        }
    }

    private void SaveState(int index, bool val)
    {
        PlayerPrefs.SetInt(PrefsPrefix + index, val ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadStates()
    {
        if (toggles == null) return;

        // Default initial states based on checklist visual target
        // Items 0, 1, 3 checked (İlaç, Su, Ateş) -> 60% default progress if no PlayerPrefs saved yet
        bool[] defaults = new bool[] { true, true, false, true, false };

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null)
            {
                string key = PrefsPrefix + i;
                if (PlayerPrefs.HasKey(key))
                {
                    toggles[i].isOn = PlayerPrefs.GetInt(key) == 1;
                }
                else if (i < defaults.Length)
                {
                    toggles[i].isOn = defaults[i];
                }
            }
        }
    }
}
