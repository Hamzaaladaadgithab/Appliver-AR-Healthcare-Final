using UnityEngine;
using UnityEngine.UI;

public class AppNavigationManager : MonoBehaviour
{
    [Header("Ekran Panelleri")]
    [Tooltip("Uygulama baslangic ekrani")]
    public GameObject splashScreen;
    
    [Tooltip("Hasta hikayesi ve bilgilendirme ekrani")]
    public GameObject patientInfoScreen;
    
    [Tooltip("Ana kontrol paneli")]
    public GameObject dashboardScreen;
    
    [Tooltip("AR Karaciger simülasyon arayüzü")]
    public GameObject arSimulationScreen;
    
    [Tooltip("Günlük bakim ve görevler ekrani")]
    public GameObject dailyCareScreen;

    [Tooltip("Doktor geçmişi ekrani")]
    public GameObject doctorHistoryScreen;

    private GameObject gameplayCanvas;

    void Awake()
    {
        // Find the gameplay Canvas dynamically
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            if (c.name != "AppScreensCanvas")
            {
                gameplayCanvas = c.gameObject;
                break;
            }
        }

        // Butonlari calisma zamaninda otomatik olarak bagla (Tak-Calistir Tasarim)
        BindButton(splashScreen, "BaslaButonu", ShowPatientInfo);
        BindButton(patientInfoScreen, "DashboardButonu", ShowDashboard);
        BindButton(patientInfoScreen, "AnaSayfayaDonButonu", ShowSplash);
        
        BindButton(dashboardScreen, "ARButonu", ShowARSimulation);
        BindButton(dashboardScreen, "DailyButonu", ShowDailyCare);
        BindButton(dashboardScreen, "HikayeButonu", ShowPatientInfo);
        
        BindButton(arSimulationScreen, "GeriButonu", ShowDashboard);
        BindButton(dailyCareScreen, "GeriButonu", ShowDashboard);
        BindButton(dashboardScreen, "DoctorHistoryButonu", ShowDoctorHistory);
        BindButton(doctorHistoryScreen, "GeriButonu", ShowDashboard);

        // Mevcut AR sahnesindeki "Ana Panele Dön" butonunu otomatik olarak bagla
        GameObject returnBtn = GameObject.Find("AnaPaneleDonButonu");
        if (returnBtn != null)
        {
            Button btn = returnBtn.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(ShowDashboard);
            }
        }
    }

    void Start()
    {
        // Uygulama basladiginda ilk olarak Splash (Giris) ekranini göster.
        ShowSplash();
    }

    /// <summary>
    /// Ekran altindaki bir butonu ismine göre bularak tıklama olayını dinamik bağlar.
    /// </summary>
    private void BindButton(GameObject screen, string buttonName, UnityEngine.Events.UnityAction action)
    {
        if (screen == null) return;
        
        Transform btnTransform = FindRecursive(screen.transform, buttonName);
        if (btnTransform != null)
        {
            Button btn = btnTransform.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
            }
        }
    }

    /// <summary>
    /// Çocuk objeler arasinda ismi eslesen objeyi özyinelemeli (recursive) arar.
    /// </summary>
    private Transform FindRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform found = FindRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>
    /// Splash (Giriş) ekranını aktif eder, diğerlerini gizler.
    /// </summary>
    public void ShowSplash()
    {
        HideAllScreens();
        if (splashScreen != null)
        {
            splashScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Hasta Hikayesi/Bilgi ekranını aktif eder, diğerlerini gizler.
    /// </summary>
    public void ShowPatientInfo()
    {
        HideAllScreens();
        if (patientInfoScreen != null)
        {
            patientInfoScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Ana Dashboard panelini aktif eder, diğerlerini gizler.
    /// </summary>
    public void ShowDashboard()
    {
        HideAllScreens();
        if (dashboardScreen != null)
        {
            dashboardScreen.SetActive(true);
        }
    }

    /// <summary>
    /// AR Simülasyon ekranını aktif eder. 
    /// Yeni ekran panellerini gizleyerek arkadaki Vuforia AR kamerasının ve mevcut AR UI elemanlarının görünmesini sağlar.
    /// </summary>
    public void ShowARSimulation()
    {
        HideAllScreens();
        if (arSimulationScreen != null)
        {
            arSimulationScreen.SetActive(true);
        }
        
        // Show gameplay Canvas for AR UI
        if (gameplayCanvas != null)
        {
            gameplayCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// Günlük Bakım/Görevler ekranını aktif eder, diğerlerini gizler.
    /// </summary>
    public void ShowDailyCare()
    {
        HideAllScreens();
        if (dailyCareScreen != null)
        {
            dailyCareScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Doktor Geçmişi ekranını aktif eder, diğerlerini gizler.
    /// </summary>
    public void ShowDoctorHistory()
    {
        HideAllScreens();
        if (doctorHistoryScreen != null)
        {
            doctorHistoryScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Tüm yeni ekran panellerini inaktif (deaktif) duruma getirir.
    /// </summary>
    public void HideAllScreens()
    {
        if (splashScreen != null) splashScreen.SetActive(false);
        if (patientInfoScreen != null) patientInfoScreen.SetActive(false);
        if (dashboardScreen != null) dashboardScreen.SetActive(false);
        if (arSimulationScreen != null) arSimulationScreen.SetActive(false);
        if (dailyCareScreen != null) dailyCareScreen.SetActive(false);
        if (doctorHistoryScreen != null) doctorHistoryScreen.SetActive(false);

        // Hide gameplay Canvas for AR UI on other screens
        if (gameplayCanvas != null)
        {
            gameplayCanvas.SetActive(false);
        }
    }
}
