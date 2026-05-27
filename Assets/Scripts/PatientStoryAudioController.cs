using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class PatientStoryAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Find the replay SpeakerButton and assign click callback dynamically
        Transform speakerBtnTransform = transform.Find("SpeakerButton");
        if (speakerBtnTransform != null)
        {
            Button speakerBtn = speakerBtnTransform.GetComponent<Button>();
            if (speakerBtn != null)
            {
                speakerBtn.onClick.AddListener(OnSpeakerButtonClick);
                
                // Find the TextMeshProUGUI text component inside SpeakerButton
                Transform textTransform = speakerBtnTransform.Find("Text");
                if (textTransform != null)
                {
                    buttonText = textTransform.GetComponent<TextMeshProUGUI>();
                }
                
                Debug.Log("PatientStoryAudioController - SpeakerButton and text bound successfully.");
            }
        }
        
        // Match UI text with actual audio playback state initially
        if (audioSource != null && audioSource.isPlaying)
        {
            UpdateButtonText("⏸ Durdur");
        }
        else
        {
            UpdateButtonText("🔊 Dinle");
        }
    }

    private void OnEnable()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            UpdateButtonText("⏸ Durdur");
            Debug.Log("PatientStoryAudioController - Audio started playing on screen enable.");
        }
    }

    private void OnDisable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            UpdateButtonText("🔊 Dinle");
            Debug.Log("PatientStoryAudioController - Audio stopped on screen disable.");
        }
    }

    private void OnSpeakerButtonClick()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                UpdateButtonText("🔊 Dinle");
                Debug.Log("PatientStoryAudioController - Audio stopped by user.");
            }
            else
            {
                audioSource.Play();
                UpdateButtonText("⏸ Durdur");
                Debug.Log("PatientStoryAudioController - Audio started by user.");
            }
        }
    }

    private void UpdateButtonText(string text)
    {
        if (buttonText == null)
        {
            // Fallback: try finding again if not cached yet
            Transform speakerBtnTransform = transform.Find("SpeakerButton");
            if (speakerBtnTransform != null)
            {
                Transform textTransform = speakerBtnTransform.Find("Text");
                if (textTransform != null)
                {
                    buttonText = textTransform.GetComponent<TextMeshProUGUI>();
                }
            }
        }

        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
}
