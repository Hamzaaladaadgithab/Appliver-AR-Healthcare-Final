using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class DoctorHistoryEntry
{
    public string icon;
    public string date;
    public string title;
    public string note;
}

public class DoctorHistoryController : MonoBehaviour
{
    [Header("UI Cards")]
    public Button[] cardButtons;
    
    [Header("Popup Panel")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupTitleText;
    public TextMeshProUGUI popupNoteText;
    public Button popupCloseButton;

    [Header("Doctor History Data")]
    public List<DoctorHistoryEntry> historyEntries = new List<DoctorHistoryEntry>()
    {
        new DoctorHistoryEntry { icon = "🩺", date = "27 Mayıs 2026", title = "Kan testi tamamlandı", note = "Hastanın ilaç uyumu iyi durumda." },
        new DoctorHistoryEntry { icon = "👨‍⚕️", date = "30 Mayıs 2026", title = "Kontrol muayenesi", note = "Kontrol sonrası mevcut ilaç dozuna devam." },
        new DoctorHistoryEntry { icon = "💊", date = "05 Haziran 2026", title = "İlaç dozu güncellendi", note = "Yeni doz ayarlaması başarıyla uygulandı." }
    };

    private void Start()
    {
        // Wire popup close button
        if (popupCloseButton != null)
        {
            popupCloseButton.onClick.RemoveAllListeners();
            popupCloseButton.onClick.AddListener(ClosePopup);
        }

        // Hide popup by default
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        // Initialize and populate cards
        PopulateCards();
    }

    public void PopulateCards()
    {
        if (cardButtons == null) return;

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i >= historyEntries.Count || cardButtons[i] == null) continue;

            DoctorHistoryEntry entry = historyEntries[i];
            
            // Set text inside the card buttons
            TextMeshProUGUI tmpText = cardButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                // Format card content nicely with inline sizing and colors
                tmpText.text = $"<size=34><color=#00F5C4>{entry.icon}  {entry.date}</color></size>\n\n<size=28>{entry.title}</size>";
            }

            // Wire click event
            int index = i; // capture index
            cardButtons[index].onClick.RemoveAllListeners();
            cardButtons[index].onClick.AddListener(() => OnCardClicked(index));
        }
    }

    private void OnCardClicked(int index)
    {
        if (index < 0 || index >= historyEntries.Count) return;
        
        DoctorHistoryEntry entry = historyEntries[index];
        
        if (popupPanel != null)
        {
            if (popupTitleText != null) popupTitleText.text = "Doktor Notu";
            if (popupNoteText != null) popupNoteText.text = entry.note;
            popupPanel.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}
