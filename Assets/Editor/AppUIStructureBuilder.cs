using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AppUIStructureBuilder : Editor
{
    [MenuItem("Appliver/Create UI Structure")]
    public static void CreateUIStructure()
    {
        // 1. AppScreensCanvas adında bir Canvas bul veya oluştur
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            canvasGO = new GameObject("AppScreensCanvas");
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create AppScreensCanvas");
        }

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        if (canvas == null) canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50; // Mevcut AR arayüzünün üzerinde durması için

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f; // Portrait mobil düzen için dengeli ölçeklendirme

        if (canvasGO.GetComponent<GraphicRaycaster>() == null)
            canvasGO.AddComponent<GraphicRaycaster>();

        // AppNavigationManager betiğini Canvas'a bağla
        AppNavigationManager navManager = canvasGO.GetComponent<AppNavigationManager>();
        if (navManager == null)
            navManager = canvasGO.AddComponent<AppNavigationManager>();

        // 2. Ana ekran panellerini oluştur veya temizleyip yeniden oluştur
        GameObject splashScreen = CreatePanel(canvasGO, "SplashScreen", new Color32(10, 15, 30, 255)); // Deep Navy
        GameObject patientInfoScreen = CreatePanel(canvasGO, "PatientInfoScreen", new Color32(10, 15, 30, 255)); // Deep Navy
        GameObject dashboardScreen = CreatePanel(canvasGO, "DashboardScreen", new Color32(15, 23, 42, 255));
        GameObject arSimulationScreen = CreatePanel(canvasGO, "ARSimulationScreen", new Color32(0, 0, 0, 0)); // Transparan (AR kamerasını görmek için)
        GameObject dailyCareScreen = CreatePanel(canvasGO, "DailyCareScreen", new Color32(15, 23, 42, 255));
        GameObject doctorHistoryScreen = CreatePanel(canvasGO, "DoctorHistoryScreen", new Color32(10, 15, 30, 255)); // Deep Navy

        // Referansları navManager üzerine ata
        navManager.splashScreen = splashScreen;
        navManager.patientInfoScreen = patientInfoScreen;
        navManager.dashboardScreen = dashboardScreen;
        navManager.arSimulationScreen = arSimulationScreen;
        navManager.dailyCareScreen = dailyCareScreen;
        navManager.doctorHistoryScreen = doctorHistoryScreen;

        // 3. Ekranları içerikle doldur
        PopulateSplashScreen(splashScreen);
        PopulatePatientInfoScreen(patientInfoScreen);
        PopulateDashboardScreen(dashboardScreen);
        PopulateARSimulationScreen(arSimulationScreen);
        PopulateDailyCareScreen(dailyCareScreen);
        PopulateDoctorHistoryScreen(doctorHistoryScreen);

        // 4. Varsayılan olarak yalnızca Splash ekranını aktif et, diğerlerini gizle
        splashScreen.SetActive(true);
        patientInfoScreen.SetActive(false);
        dashboardScreen.SetActive(false);
        arSimulationScreen.SetActive(false);
        dailyCareScreen.SetActive(false);
        doctorHistoryScreen.SetActive(false);

        EditorUtility.SetDirty(navManager);
        Debug.Log("Appliver Mobil UI Arayüz Yapısı başarıyla kuruldu! Play'e basarak test edebilirsiniz.");
    }

    [MenuItem("Appliver/Apply Splash UI")]
    public static void ApplySplashUI()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. SplashScreen panelini bul
        Transform splashTransform = canvasGO.transform.Find("SplashScreen");
        if (splashTransform == null)
        {
            Debug.LogError("SplashScreen paneli AppScreensCanvas altında bulunamadı!");
            return;
        }

        GameObject splashScreen = splashTransform.gameObject;
        Undo.RegisterCompleteObjectUndo(splashScreen, "Apply Splash UI");

        // Background color of SplashScreen panel set to premium dark navy
        Image bgImage = splashScreen.GetComponent<Image>();
        if (bgImage == null) bgImage = splashScreen.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // 3. Clear existing children under SplashScreen to rebuild cleanly
        for (int i = splashScreen.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(splashScreen.transform.GetChild(i).gameObject);
        }

        // 4. Rebuild the visual elements inside SplashScreen
        // Ana Başlık
        CreateText(splashScreen, "Baslik", "APPLIVER", 80, TextAlignmentOptions.Center,
            new Vector2(0.1f, 0.65f), new Vector2(0.9f, 0.85f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Canlı Mint Yeşili

        // Alt Başlık (Karaciğer Nakli Hasta Takip ve Destek Sistemi + Güvenli takip • ilaç uyumu • destek)
        string altBaslikText = "Karaciğer Nakli Hasta Takip\nve Destek Sistemi\n\n<size=28><color=#64748B>Güvenli takip  •  ilaç uyumu  •  destek</color></size>";
        CreateText(splashScreen, "AltBaslik", altBaslikText, 36, TextAlignmentOptions.Center,
            new Vector2(0.1f, 0.38f), new Vector2(0.9f, 0.60f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(226, 232, 240, 255)); // Slate 200

        // Başla Butonu (TAKİBE BAŞLA)
        CreateButton(splashScreen, "BaslaButonu", "TAKİBE BAŞLA",
            new Vector2(0.5f, 0.20f), new Vector2(0.5f, 0.20f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(550, 120), new Color32(14, 165, 233, 255)); // Modern Medikal Mavi

        EditorUtility.SetDirty(splashScreen);
        Debug.Log("Appliver - Apply Splash UI completed successfully! SplashScreen has been redesigned to look like a premium hospital-grade medical mobile welcome screen.");
    }

    [MenuItem("Appliver/Apply Patient Info UI")]
    public static void ApplyPatientInfoUI()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. PatientInfoScreen panelini bul
        Transform infoTransform = canvasGO.transform.Find("PatientInfoScreen");
        if (infoTransform == null)
        {
            Debug.LogError("PatientInfoScreen paneli AppScreensCanvas altında bulunamadı!");
            return;
        }

        GameObject patientInfo = infoTransform.gameObject;
        Undo.RegisterCompleteObjectUndo(patientInfo, "Apply Patient Info UI");

        // Background color of PatientInfoScreen panel set to premium dark navy
        Image bgImage = patientInfo.GetComponent<Image>();
        if (bgImage == null) bgImage = patientInfo.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // 3. Clear existing children under PatientInfoScreen to rebuild cleanly
        for (int i = patientInfo.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(patientInfo.transform.GetChild(i).gameObject);
        }

        // 4. Rebuild the visual elements inside PatientInfoScreen
        // Ekran Başlığı (Move slightly lower, reduce font size to prevent top clipping)
        CreateText(patientInfo, "Baslik", "HAMZA'NIN HİKAYESİ", 48, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255));

        // Kart 1: Nakil Süreci (Y-anchor: 0.64 to 0.79)
        CreateCardWithTitleAndText(patientInfo, "NakilKarti", "1. Nakil Süreci",
            "Hamza, yakın zamanda başarılı bir karaciğer nakli operasyonu geçirdi. Yeni organının sağlıklı çalışması günlük bakımına bağlıdır.",
            new Vector2(0.08f, 0.64f), new Vector2(0.92f, 0.79f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // Kart 2: İlaç Takibi (Y-anchor: 0.47 to 0.62)
        CreateCardWithTitleAndText(patientInfo, "TakipKarti", "2. İlaç Takibi",
            "Bağışıklık sisteminin yeni karaciğeri reddetmemesi için koruyucu ilaçların her gün tam zamanında ve eksiksiz alınması kritik önem taşır.",
            new Vector2(0.08f, 0.47f), new Vector2(0.92f, 0.62f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // Kart 3: Organ Reddi Riski (Y-anchor: 0.30 to 0.45)
        CreateCardWithTitleAndText(patientInfo, "RiskKarti", "3. Organ Reddi Riski",
            "İlaç dozlarının kaçırılması veya geciktirilmesi organ reddi reaksiyonunu tetikleyebilir. Bu durum hayati tehlike oluşturur!",
            new Vector2(0.08f, 0.30f), new Vector2(0.92f, 0.45f),
            new Color32(239, 68, 68, 255)); // Vivid Red

        // Replay Butonu (SpeakerButton) - Shifted to Y: 0.24, Size: 380x75
        CreateButton(patientInfo, "SpeakerButton", "🔊 Dinle",
            new Vector2(0.5f, 0.24f), new Vector2(0.5f, 0.24f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(380, 75), new Color32(71, 85, 105, 255)); // Koyu Slate

        // İleri Butonu (DashboardButonu) - Shifted to Y: 0.15, Size: 580x100
        CreateButton(patientInfo, "DashboardButonu", "KONTROL PANELİNE GİT",
            new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(580, 100), new Color32(14, 165, 233, 255)); // Modern Medikal Mavi

        // Audio support setup
        PatientStoryAudioController audioController = patientInfo.GetComponent<PatientStoryAudioController>();
        if (audioController == null)
        {
            audioController = patientInfo.AddComponent<PatientStoryAudioController>();
        }

        AudioSource audioSource = patientInfo.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/hamza_story.wav");
            if (clip != null)
            {
                audioSource.clip = clip;
                Debug.Log("Appliver - hamza_story.wav successfully assigned to PatientInfoScreen AudioSource.");
            }
            else
            {
                Debug.LogWarning("Appliver - Assets/Audio/hamza_story.wav not found! Please ensure the file exists.");
            }
        }

        EditorUtility.SetDirty(patientInfo);
        Debug.Log("Appliver - Apply Patient Info UI completed successfully! PatientInfoScreen has been redesigned into 3 rounded cards with audio playback controls.");
    }

    [MenuItem("Appliver/Apply Patient Back Button")]
    public static void ApplyPatientBackButton()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. PatientInfoScreen panelini bul
        Transform infoTransform = canvasGO.transform.Find("PatientInfoScreen");
        if (infoTransform == null)
        {
            Debug.LogError("PatientInfoScreen paneli AppScreensCanvas altında bulunamadı!");
            return;
        }

        GameObject patientInfo = infoTransform.gameObject;
        Undo.RegisterCompleteObjectUndo(patientInfo, "Apply Patient Back Button");

        // 3. Check if button already exists, delete to rebuild cleanly
        Transform existingBtn = infoTransform.Find("AnaSayfayaDonButonu");
        if (existingBtn != null)
        {
            DestroyImmediate(existingBtn.gameObject);
        }

        // 4. Create the button (AnaSayfayaDonButonu)
        GameObject btnGO = new GameObject("AnaSayfayaDonButonu");
        btnGO.transform.SetParent(patientInfo.transform, false);

        RectTransform rect = btnGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(25f, -25f);
        rect.sizeDelta = new Vector2(220f, 60f);

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color32(30, 41, 59, 255); // Dark Slate 800

        // Use Unity's built-in rounded UISprite for a modern pill look
        Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSprite != null)
        {
            img.sprite = roundedSprite;
            img.type = Image.Type.Sliced;
        }

        btnGO.AddComponent<Button>();

        // Text inside the button
        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);

        RectTransform txtRect = txtGO.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = txtGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "← ANA SAYFAYA DÖN";
        tmp.fontSize = 20; // safe compact font size for 220 width
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;

        EditorUtility.SetDirty(patientInfo);
        Debug.Log("Appliver - Apply Patient Back Button completed successfully! AnaSayfayaDonButonu has been created on PatientInfoScreen.");
    }

    [MenuItem("Appliver/Apply Dashboard UI")]
    public static void ApplyDashboardUI()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. DashboardScreen panelini bul
        Transform dashTransform = canvasGO.transform.Find("DashboardScreen");
        if (dashTransform == null)
        {
            Debug.LogError("DashboardScreen paneli AppScreensCanvas altında bulunamadı!");
            return;
        }

        GameObject dashboardScreen = dashTransform.gameObject;
        Undo.RegisterCompleteObjectUndo(dashboardScreen, "Apply Dashboard UI");

        // Background color of DashboardScreen panel set to premium dark navy
        Image bgImage = dashboardScreen.GetComponent<Image>();
        if (bgImage == null) bgImage = dashboardScreen.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // 3. Clear existing children under DashboardScreen to rebuild cleanly
        for (int i = dashboardScreen.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(dashboardScreen.transform.GetChild(i).gameObject);
        }

        // 4. Rebuild the visual elements inside DashboardScreen
        // Ekran Başlığı (Merhaba Hamza 👋)
        CreateText(dashboardScreen, "Baslik", "Merhaba Hamza 👋", 48, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Canlı Mint Yeşili

        // Alt Başlık (Bugünkü takip durumun)
        CreateText(dashboardScreen, "AltBaslik", "Bugünkü takip durumun", 28, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.76f), new Vector2(0.95f, 0.82f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(148, 163, 184, 255)); // Slate 400

        // Takip Özet Kartı
        string durumOzeti = "Sağlık Durumu: İYİ\nRisk Durumu: DÜŞÜK\nSon İlaç: 21:30";
        CreateCardWithTitleAndText(dashboardScreen, "DurumKarti", "Takip Özetiniz", durumOzeti,
            new Vector2(0.08f, 0.54f), new Vector2(0.92f, 0.72f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // 1. ARButonu (AR SİMÜLASYON) - Center Y: 0.43, Size: 650x75, Color: Medical Blue (#0EA5E9)
        CreateButton(dashboardScreen, "ARButonu", "AR SİMÜLASYON",
            new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(14, 165, 233, 255)); // Medical Blue

        // 2. DailyButonu (GÜNLÜK GÖREVLER) - Center Y: 0.34, Size: 650x75, Color: Emerald Green (#10B981)
        CreateButton(dashboardScreen, "DailyButonu", "GÜNLÜK GÖREVLER",
            new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(16, 185, 129, 255)); // Emerald Green

        // 3. DoctorHistoryButonu (DOKTOR GEÇMİŞİ) - Center Y: 0.25, Size: 650x75, Color: Slate Gray (#475569)
        CreateButton(dashboardScreen, "DoctorHistoryButonu", "DOKTOR GEÇMİŞİ",
            new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(71, 85, 105, 255)); // Slate Gray

        // 4. HikayeButonu (HİKAYEYİ TEKRAR OKU) - Center Y: 0.16, Size: 650x75, Color: Purple / Indigo (#6366F1)
        CreateButton(dashboardScreen, "HikayeButonu", "HİKAYEYİ TEKRAR OKU",
            new Vector2(0.5f, 0.16f), new Vector2(0.5f, 0.16f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(99, 102, 241, 255)); // Purple / Indigo

        EditorUtility.SetDirty(dashboardScreen);
        Debug.Log("Appliver - Apply Dashboard UI completed successfully! DashboardScreen has been redesigned into a premium patient tracking dashboard.");
    }

    [MenuItem("Appliver/Apply Daily Care UI")]
    public static void ApplyDailyCareUI()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. DailyCareScreen panelini bul
        Transform dailyTransform = canvasGO.transform.Find("DailyCareScreen");
        if (dailyTransform == null)
        {
            Debug.LogError("DailyCareScreen paneli AppScreensCanvas altında bulunamadı!");
            return;
        }

        GameObject dailyCareScreen = dailyTransform.gameObject;
        Undo.RegisterCompleteObjectUndo(dailyCareScreen, "Apply Daily Care UI");

        // Background color of DailyCareScreen set to premium dark navy
        Image bgImage = dailyCareScreen.GetComponent<Image>();
        if (bgImage == null) bgImage = dailyCareScreen.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // 3. Clear existing children under DailyCareScreen to rebuild cleanly
        for (int i = dailyCareScreen.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(dailyCareScreen.transform.GetChild(i).gameObject);
        }

        // 4. Rebuild the visual elements inside DailyCareScreen
        // Ekran Başlığı (Baslik): GÜNLÜK BAKIM GÖREVLERİ
        CreateText(dailyCareScreen, "Baslik", "GÜNLÜK BAKIM GÖREVLERİ", 44, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Mint Green

        // Görev Listesi Kartı (GorevKarti)
        GameObject listCard = CreatePanel(dailyCareScreen, "GorevKarti", new Color32(30, 41, 59, 255)); // Slate 800
        Image cardImg = listCard.GetComponent<Image>();
        if (cardImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                cardImg.sprite = roundedSprite;
                cardImg.type = Image.Type.Sliced;
            }
        }
        RectTransform listRect = listCard.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0.08f, 0.26f);
        listRect.anchorMax = new Vector2(0.92f, 0.78f);
        listRect.offsetMin = Vector2.zero;
        listRect.offsetMax = Vector2.zero;

        // Build 5 separate interactive Toggles inside GorevKarti
        GameObject toggle0 = CreateToggleRow(listCard, "Toggle_0", "İlaç alındı", new Vector2(0.05f, 0.792f), new Vector2(0.95f, 0.92f));
        GameObject toggle1 = CreateToggleRow(listCard, "Toggle_1", "Su içildi", new Vector2(0.05f, 0.664f), new Vector2(0.95f, 0.792f));
        GameObject toggle2 = CreateToggleRow(listCard, "Toggle_2", "Hafif yürüyüş", new Vector2(0.05f, 0.536f), new Vector2(0.95f, 0.664f));
        GameObject toggle3 = CreateToggleRow(listCard, "Toggle_3", "Ateş kontrolü", new Vector2(0.05f, 0.408f), new Vector2(0.95f, 0.536f));
        GameObject toggle4 = CreateToggleRow(listCard, "Toggle_4", "Hijyen", new Vector2(0.05f, 0.28f), new Vector2(0.95f, 0.408f));

        // Progress Bar Container (ProgressContainer)
        GameObject progressContainer = CreatePanel(listCard, "ProgressContainer", new Color32(0, 0, 0, 0));
        RectTransform progressContainerRect = progressContainer.GetComponent<RectTransform>();
        progressContainerRect.anchorMin = new Vector2(0.08f, 0.05f);
        progressContainerRect.anchorMax = new Vector2(0.92f, 0.24f);
        progressContainerRect.offsetMin = Vector2.zero;
        progressContainerRect.offsetMax = Vector2.zero;

        // Progress Text: "Günlük Uyum: %0"
        TextMeshProUGUI progressTextTmp = CreateText(progressContainer, "ProgressText", "Günlük Uyum: %0", 26, TextAlignmentOptions.Left,
            new Vector2(0f, 0.55f), new Vector2(1f, 1.0f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(226, 232, 240, 255)); // Slate 200

        // Progress Track Background
        GameObject progressTrack = CreatePanel(progressContainer, "ProgressTrack", new Color32(15, 23, 42, 255)); // Deep Slate 900
        Image trackImg = progressTrack.GetComponent<Image>();
        if (trackImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                trackImg.sprite = roundedSprite;
                trackImg.type = Image.Type.Sliced;
            }
        }
        RectTransform trackRect = progressTrack.GetComponent<RectTransform>();
        trackRect.anchorMin = new Vector2(0f, 0.15f);
        trackRect.anchorMax = new Vector2(1f, 0.45f);
        trackRect.offsetMin = Vector2.zero;
        trackRect.offsetMax = Vector2.zero;

        // Progress Fill Bar (Mint Green, Filled type for dynamic updates)
        GameObject progressFill = CreatePanel(progressTrack, "ProgressFill", new Color32(0, 245, 196, 255)); // Mint Green
        Image fillImg = progressFill.GetComponent<Image>();
        if (fillImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                fillImg.sprite = roundedSprite;
            }
            fillImg.type = Image.Type.Filled;
            fillImg.fillMethod = Image.FillMethod.Horizontal;
            fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImg.fillAmount = 0.6f;
        }
        RectTransform fillRect = progressFill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        // Geri Butonu (GeriButonu) - Shifted Y to 0.18 for better visibility, Size: 500x90, Azure Blue (#0EA5E9)
        CreateButton(dailyCareScreen, "GeriButonu", "GERİ",
            new Vector2(0.5f, 0.18f), new Vector2(0.5f, 0.18f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(500, 90), new Color32(14, 165, 233, 255));

        // 5. Setup controller and assign references
        DailyCareController controller = dailyCareScreen.GetComponent<DailyCareController>();
        if (controller == null) controller = dailyCareScreen.AddComponent<DailyCareController>();

        controller.toggles = new Toggle[5] {
            toggle0.GetComponent<Toggle>(),
            toggle1.GetComponent<Toggle>(),
            toggle2.GetComponent<Toggle>(),
            toggle3.GetComponent<Toggle>(),
            toggle4.GetComponent<Toggle>()
        };
        controller.progressText = progressTextTmp;
        controller.progressFillImage = fillImg;

        EditorUtility.SetDirty(controller);
        EditorUtility.SetDirty(dailyCareScreen);
        Debug.Log("Appliver - Apply Daily Care UI completed successfully! DailyCareScreen is now interactive and styled professionally.");
    }

    [MenuItem("Appliver/Apply Doctor History UI")]
    public static void ApplyDoctorHistoryUI()
    {
        // 1. AppScreensCanvas adında bir Canvas bul
        GameObject canvasGO = GameObject.Find("AppScreensCanvas");
        if (canvasGO == null)
        {
            Debug.LogError("AppScreensCanvas sahnede bulunamadı!");
            return;
        }

        // 2. DoctorHistoryScreen panelini bul veya oluştur
        Transform docTransform = canvasGO.transform.Find("DoctorHistoryScreen");
        GameObject doctorHistoryScreen;
        if (docTransform == null)
        {
            doctorHistoryScreen = CreatePanel(canvasGO, "DoctorHistoryScreen", new Color32(10, 15, 30, 255));
        }
        else
        {
            doctorHistoryScreen = docTransform.gameObject;
        }

        Undo.RegisterCompleteObjectUndo(doctorHistoryScreen, "Apply Doctor History UI");

        // Background color of DoctorHistoryScreen set to premium dark navy
        Image bgImage = doctorHistoryScreen.GetComponent<Image>();
        if (bgImage == null) bgImage = doctorHistoryScreen.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // 3. Clear existing children under DoctorHistoryScreen to rebuild cleanly
        for (int i = doctorHistoryScreen.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(doctorHistoryScreen.transform.GetChild(i).gameObject);
        }

        // 4. Populate screen
        PopulateDoctorHistoryScreen(doctorHistoryScreen);

        // 5. Connect reference in AppNavigationManager if present
        AppNavigationManager navManager = canvasGO.GetComponent<AppNavigationManager>();
        if (navManager != null)
        {
            navManager.doctorHistoryScreen = doctorHistoryScreen;
            EditorUtility.SetDirty(navManager);
        }

        EditorUtility.SetDirty(doctorHistoryScreen);
        Debug.Log("Appliver - Apply Doctor History UI completed successfully! DoctorHistoryScreen has been constructed.");
    }

    [MenuItem("Appliver/Apply Clean AR UI Layout")]
    public static void ApplyCleanARUILayout()
    {
        // 1. Mevcut oyun arayüzünü barındıran asıl Canvas'ı bul (AppScreensCanvas olmayan Canvas)
        Canvas gameplayCanvas = FindGameplayCanvas();
        if (gameplayCanvas == null)
        {
            Debug.LogError("AR oyun arayüzünü barındıran ana Canvas sahnede bulunamadı! Lütfen mevcut Canvas ismini kontrol edin.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(gameplayCanvas.gameObject, "Apply Clean AR UI Layout");

        // Remove any layout groups causing bad sizing
        LayoutGroup[] lgs = gameplayCanvas.GetComponentsInChildren<LayoutGroup>(true);
        foreach (LayoutGroup lg in lgs)
        {
            DestroyImmediate(lg);
        }

        // 2. GameManager ve ilgili betiklerden UI referanslarını al
        GameObject gameManager = GameObject.Find("GameManager");
        ScenarioManager scenarioManager = null;
        ScoreManager scoreManager = null;
        UIManager uiManager = null;

        if (gameManager != null)
        {
            scenarioManager = gameManager.GetComponent<ScenarioManager>();
            scoreManager = gameManager.GetComponent<ScoreManager>();
            uiManager = gameManager.GetComponent<UIManager>();
        }

        // Skor elemanlarını tespit et
        Text skorText = scoreManager != null ? scoreManager.skorText : (FindComponentInCanvas<Text>(gameplayCanvas, "SkorText") ?? FindComponentInCanvas<Text>(gameplayCanvas, "skorText"));
        Text dogruText = scoreManager != null ? scoreManager.dogruText : (FindComponentInCanvas<Text>(gameplayCanvas, "DogruText") ?? FindComponentInCanvas<Text>(gameplayCanvas, "dogruText"));
        Text yanlisText = scoreManager != null ? scoreManager.yanlisText : (FindComponentInCanvas<Text>(gameplayCanvas, "YanlisText") ?? FindComponentInCanvas<Text>(gameplayCanvas, "yanlisText"));

        // Panelleri tespit et
        GameObject kararPanel = scenarioManager != null ? scenarioManager.kararPanel : (FindObjectInCanvas(gameplayCanvas, "kararPanel") ?? FindObjectInCanvas(gameplayCanvas, "KararPanel"));
        GameObject bildirimPanel = scenarioManager != null ? scenarioManager.bildirimPanel : (FindObjectInCanvas(gameplayCanvas, "bildirimPanel") ?? FindObjectInCanvas(gameplayCanvas, "BildirimPanel"));

        // Metin elemanlarını tespit et
        Text bildirimText = scenarioManager != null ? scenarioManager.bildirimText : (FindComponentInCanvas<Text>(gameplayCanvas, "bildirimText") ?? FindComponentInCanvas<Text>(gameplayCanvas, "BildirimText"));
        TextMeshProUGUI gecmisText = scenarioManager != null ? scenarioManager.gecmisText : (FindComponentInCanvas<TextMeshProUGUI>(gameplayCanvas, "gecmisText") ?? FindComponentInCanvas<TextMeshProUGUI>(gameplayCanvas, "GecmisText"));

        // Karar butonlarını tespit et
        Button butonA = scenarioManager != null ? scenarioManager.butonA : (FindComponentInCanvas<Button>(gameplayCanvas, "butonA") ?? FindComponentInCanvas<Button>(gameplayCanvas, "ButonA"));
        Button butonB = scenarioManager != null ? scenarioManager.butonB : (FindComponentInCanvas<Button>(gameplayCanvas, "butonB") ?? FindComponentInCanvas<Button>(gameplayCanvas, "ButonB"));

        if (butonA == null && uiManager != null) butonA = uiManager.butonA;
        if (butonB == null && uiManager != null) butonB = uiManager.butonB;

        // 3. Script referanslarını topla ve gereksiz dekoratif/kopya objeleri temizle
        HashSet<GameObject> referencedObjects = new HashSet<GameObject>();
        referencedObjects.Add(gameplayCanvas.gameObject);
        if (kararPanel != null) referencedObjects.Add(kararPanel);
        if (bildirimPanel != null) referencedObjects.Add(bildirimPanel);
        if (bildirimText != null) referencedObjects.Add(bildirimText.gameObject);
        if (gecmisText != null) referencedObjects.Add(gecmisText.gameObject);
        if (butonA != null) referencedObjects.Add(butonA.gameObject);
        if (butonB != null) referencedObjects.Add(butonB.gameObject);
        if (skorText != null) referencedObjects.Add(skorText.gameObject);
        if (dogruText != null) referencedObjects.Add(dogruText.gameObject);
        if (yanlisText != null) referencedObjects.Add(yanlisText.gameObject);

        GameObject scorePanel = FindObjectInCanvas(gameplayCanvas, "SkorPanel")
            ?? FindObjectInCanvas(gameplayCanvas, "skorPanel")
            ?? FindObjectInCanvas(gameplayCanvas, "SkorPanel ");
        if (scorePanel != null) referencedObjects.Add(scorePanel);

        // Gereksiz objeleri sil (New Text, mavi dikdörtgenler, gri arka planlar vb.)
        CleanTargetedUselessObjects(gameplayCanvas.transform, referencedObjects);

        // Remove ONLY static instruction UI
        GameObject appScreensCanvas = GameObject.Find("AppScreensCanvas");
        if (appScreensCanvas != null)
        {
            Transform bilgiMetni = FindRecursive(appScreensCanvas.transform, "BilgiMetni");
            if (bilgiMetni != null)
            {
                bilgiMetni.gameObject.SetActive(false);
                Debug.Log("Appliver Cleanup - Disabled static BilgiMetni.");
            }
        }

        // Sahne kök dizinindeki olası başıboş objeleri de sil
        GameObject stray1 = GameObject.Find("New Text");
        if (stray1 != null) DestroyImmediate(stray1);
        GameObject stray2 = GameObject.Find("New Text (TMP)");
        if (stray2 != null) DestroyImmediate(stray2);

        // 4. Gameplay Canvas Scaler'ı mobil dikey portrait için yapılandır ve ölçeği / sırayı düzelt
        gameplayCanvas.sortingOrder = 60; // Render on top of AppScreensCanvas
        gameplayCanvas.gameObject.layer = 5; // UI Layer
        RectTransform gameplayCanvasRect = gameplayCanvas.GetComponent<RectTransform>();
        if (gameplayCanvasRect != null)
        {
            gameplayCanvasRect.localScale = Vector3.one;
        }

        CanvasScaler scaler = gameplayCanvas.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = gameplayCanvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;


        // 5. Skor Panelini en üstte yatay konumlandır (Height: 120)
        ReorganizeScorePanel(gameplayCanvas.gameObject, ref skorText, ref dogruText, ref yanlisText, scoreManager);

        // 6. Bildirim Panelini üst-orta bölüme konumlandır
        ReorganizeNotificationPanel(bildirimPanel, bildirimText, gameplayCanvas.gameObject);

        // 7. Karar/Senaryo Panelini alt-orta bölüme konumlandır (Width: 1600, Height: 420)
        ReorganizeDecisionPanel(kararPanel, gecmisText);

        // 8. Karar butonlarını KararPanel'in içerisine dikey yerleşim çakışması olmadan yerleştir (Size: 550x90, horizontal)
        ReorganizeDecisionButtons(kararPanel, butonA, butonB);

        // 9. "Ana Panele Dön" Butonunu sol üste yerleştir (Boyut: 260x70)
        CreateDashboardReturnButton(gameplayCanvas.gameObject);

        // Rebind references only if they are missing
        if (scoreManager != null)
        {
            if (scoreManager.skorText == null) scoreManager.skorText = skorText;
            if (scoreManager.dogruText == null) scoreManager.dogruText = dogruText;
            if (scoreManager.yanlisText == null) scoreManager.yanlisText = yanlisText;
            EditorUtility.SetDirty(scoreManager);
        }
        if (scenarioManager != null)
        {
            if (scenarioManager.bildirimPanel == null) scenarioManager.bildirimPanel = bildirimPanel;
            if (scenarioManager.bildirimText == null) scenarioManager.bildirimText = bildirimText;
            if (scenarioManager.kararPanel == null) scenarioManager.kararPanel = kararPanel;
            if (scenarioManager.gecmisText == null) scenarioManager.gecmisText = gecmisText;
            if (scenarioManager.butonA == null) scenarioManager.butonA = butonA;
            if (scenarioManager.butonB == null) scenarioManager.butonB = butonB;
            EditorUtility.SetDirty(scenarioManager);
        }

        EditorUtility.SetDirty(gameplayCanvas);
        if (gameManager != null) EditorUtility.SetDirty(gameManager);

        Debug.Log("Apply Clean AR UI Layout completed successfully! All Canvas, Panel, and Button parameters have been aligned with the target reference design.");
    }

    private static Canvas FindGameplayCanvas()
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            if (c.name != "AppScreensCanvas") return c;
        }
        return null;
    }

    private static T FindComponentInCanvas<T>(Canvas canvas, string name) where T : Component
    {
        Transform t = FindRecursive(canvas.transform, name);
        if (t != null) return t.GetComponent<T>();
        return null;
    }

    private static GameObject FindObjectInCanvas(Canvas canvas, string name)
    {
        Transform t = FindRecursive(canvas.transform, name);
        if (t != null) return t.gameObject;
        return null;
    }

    private static void CleanTargetedUselessObjects(Transform parent, HashSet<GameObject> referenced)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            // Eğer script referansı olan bir obje ise koru ve çocuklarını tara
            if (referenced.Contains(child.gameObject))
            {
                CleanTargetedUselessObjects(child, referenced);
                continue;
            }

            string lowercaseName = child.name.ToLower();
            bool shouldDelete = false;

            // 1. Kopya "New Text" objeleri
            if (lowercaseName.Contains("new text") || lowercaseName.Contains("newtext"))
            {
                shouldDelete = true;
            }
            // 2. Mavi dekoratif dikdörtgenler (referansı olmayan)
            else if (lowercaseName.Contains("blue") || lowercaseName.Contains("rect") || lowercaseName.Contains("mavi") || lowercaseName.Contains("kutu") || lowercaseName.Contains("horizontal") || lowercaseName.Contains("decor") || lowercaseName.Contains("rectangle"))
            {
                shouldDelete = true;
            }
            // 3. Kullanılmayan gri arka plan veya paneller (empty Image panels around the upper-middle and middle area)
            else if (lowercaseName.Contains("gray") || lowercaseName.Contains("grey") || lowercaseName.Contains("panel") || lowercaseName.Contains("background") || lowercaseName.Contains("overlay") || lowercaseName.Contains("gri") || lowercaseName.Contains("gorsel") || lowercaseName.Contains("image") || child.GetComponent<Image>() != null)
            {
                // Karar, bildirim, score veya dön butonları değilse sil
                if (child.name != "kararPanel" && child.name != "KararPanel" &&
                    child.name != "bildirimPanel" && child.name != "BildirimPanel" &&
                    child.name != "AnaPaneleDonButonu" &&
                    child.name != "SkorPanel" && child.name != "skorPanel" && child.name != "SkorPanel ")
                {
                    shouldDelete = true;
                }
            }

            if (shouldDelete)
            {
                Debug.Log("Appliver Cleanup - Deleted unreferenced UI element: " + child.name);
                DestroyImmediate(child.gameObject);
            }
            else
            {
                CleanTargetedUselessObjects(child, referenced);
            }
        }
    }

    private static void DisableTopInstructionText(Canvas canvas)
    {
        Text[] texts = canvas.GetComponentsInChildren<Text>(true);
        foreach (Text t in texts)
        {
            string txtVal = t.text;
            if (txtVal.Contains("Kamerayı") || txtVal.Contains("Kamerayi") || txtVal.Contains("İşaretçisine") || txtVal.Contains("Isaretcisine") || txtVal.Contains("Tutun"))
            {
                t.gameObject.SetActive(false);
                Debug.Log("Appliver Cleanup - Disabled top Text instruction GameObject: " + t.gameObject.name);
            }
        }

        TextMeshProUGUI[] tmps = canvas.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI tmp in tmps)
        {
            string txtVal = tmp.text;
            if (txtVal.Contains("Kamerayı") || txtVal.Contains("Kamerayi") || txtVal.Contains("İşaretçisine") || txtVal.Contains("Isaretcisine") || txtVal.Contains("Tutun"))
            {
                tmp.gameObject.SetActive(false);
                Debug.Log("Appliver Cleanup - Disabled top TMP instruction GameObject: " + tmp.gameObject.name);
            }
        }
    }

    private static void ReorganizeScorePanel(GameObject canvas, ref Text skor, ref Text dogru, ref Text yanlis, ScoreManager scoreManager)
    {
        // 1. Sahnede var olan SkorPanel'i tespit et veya oluştur
        GameObject scorePanel = FindObjectInCanvas(canvas.GetComponent<Canvas>(), "SkorPanel")
            ?? FindObjectInCanvas(canvas.GetComponent<Canvas>(), "skorPanel")
            ?? FindObjectInCanvas(canvas.GetComponent<Canvas>(), "SkorPanel ");

        if (scorePanel == null)
        {
            scorePanel = new GameObject("SkorPanel");
            scorePanel.transform.SetParent(canvas.transform, false);
        }
        scorePanel.layer = 5; // UI Layer


        RectTransform pRect = scorePanel.GetComponent<RectTransform>();
        if (pRect == null) pRect = scorePanel.AddComponent<RectTransform>();

        LayoutGroup lg = scorePanel.GetComponent<LayoutGroup>();
        if (lg != null) DestroyImmediate(lg);

        pRect.anchorMin = new Vector2(0f, 1f);
        pRect.anchorMax = new Vector2(1f, 1f);
        pRect.pivot = new Vector2(0.5f, 1f);
        pRect.offsetMin = new Vector2(330f, -105f); // Left = 330, Top = 25, Height = 80
        pRect.offsetMax = new Vector2(-30f, -25f);  // Right = 30, Top = 25


        Image pImg = scorePanel.GetComponent<Image>();
        if (pImg == null) pImg = scorePanel.AddComponent<Image>();
        pImg.color = new Color32(15, 23, 42, 220); // Şeffaf koyu slate 900

        Transform parentTransform = scorePanel.transform;

        // 2. Eksik veya silinmiş skor metin objeleri varsa onları onar ve yeniden oluştur
        if (skor == null)
        {
            GameObject go = new GameObject("skorText");
            skor = go.AddComponent<Text>();
            if (scoreManager != null) scoreManager.skorText = skor;
            Debug.Log("Appliver Repair - 'skorText' otomatik olarak yeniden oluşturuldu.");
        }
        if (dogru == null)
        {
            GameObject go = new GameObject("dogruText");
            dogru = go.AddComponent<Text>();
            if (scoreManager != null) scoreManager.dogruText = dogru;
            Debug.Log("Appliver Repair - 'dogruText' otomatik olarak yeniden oluşturuldu.");
        }
        if (yanlis == null)
        {
            GameObject go = new GameObject("yanlisText");
            yanlis = go.AddComponent<Text>();
            if (scoreManager != null) scoreManager.yanlisText = yanlis;
            Debug.Log("Appliver Repair - 'yanlisText' otomatik olarak yeniden oluşturuldu.");
        }

        // 3. Skor metnini konumlandır ve biçimlendir (Sol sütun, eşit paylaşımlı)
        if (skor != null)
        {
            skor.gameObject.layer = 5; // UI Layer
            skor.transform.SetParent(parentTransform, false);
            RectTransform rSkor = skor.GetComponent<RectTransform>();
            rSkor.anchorMin = new Vector2(0f, 0f);
            rSkor.anchorMax = new Vector2(0.33f, 1f);
            rSkor.pivot = new Vector2(0.5f, 0.5f);
            rSkor.anchoredPosition = Vector2.zero;
            rSkor.sizeDelta = Vector2.zero;

            skor.horizontalOverflow = HorizontalWrapMode.Overflow;
            skor.verticalOverflow = VerticalWrapMode.Overflow;
            skor.alignment = TextAnchor.MiddleCenter;
            skor.fontSize = 30;
            skor.fontStyle = FontStyle.Bold;
            skor.color = Color.white;
            skor.transform.localScale = Vector3.one;
            skor.transform.localRotation = Quaternion.identity;
        }

        // 4. Doğru metnini konumlandır ve biçimlendir (Orta sütun, eşit paylaşımlı)
        if (dogru != null)
        {
            dogru.gameObject.layer = 5; // UI Layer
            dogru.transform.SetParent(parentTransform, false);
            RectTransform rDogru = dogru.GetComponent<RectTransform>();
            rDogru.anchorMin = new Vector2(0.33f, 0f);
            rDogru.anchorMax = new Vector2(0.66f, 1f);
            rDogru.pivot = new Vector2(0.5f, 0.5f);
            rDogru.anchoredPosition = Vector2.zero;
            rDogru.sizeDelta = Vector2.zero;

            dogru.horizontalOverflow = HorizontalWrapMode.Overflow;
            dogru.verticalOverflow = VerticalWrapMode.Overflow;
            dogru.alignment = TextAnchor.MiddleCenter;
            dogru.fontSize = 30;
            dogru.fontStyle = FontStyle.Bold;
            dogru.color = new Color32(34, 197, 94, 255); // Doğru: Yeşil
            dogru.transform.localScale = Vector3.one;
            dogru.transform.localRotation = Quaternion.identity;
        }

        // 5. Yanlış metnini konumlandır ve biçimlendir (Sağ sütun, eşit paylaşımlı)
        if (yanlis != null)
        {
            yanlis.gameObject.layer = 5; // UI Layer
            yanlis.transform.SetParent(parentTransform, false);
            RectTransform rYanlis = yanlis.GetComponent<RectTransform>();
            rYanlis.anchorMin = new Vector2(0.66f, 0f);
            rYanlis.anchorMax = new Vector2(1f, 1f);
            rYanlis.pivot = new Vector2(0.5f, 0.5f);
            rYanlis.anchoredPosition = Vector2.zero;
            rYanlis.sizeDelta = Vector2.zero;

            yanlis.horizontalOverflow = HorizontalWrapMode.Overflow;
            yanlis.verticalOverflow = VerticalWrapMode.Overflow;
            yanlis.alignment = TextAnchor.MiddleCenter;
            yanlis.fontSize = 30;
            yanlis.fontStyle = FontStyle.Bold;
            yanlis.color = new Color32(239, 68, 68, 255); // Yanlış: Kırmızı
            yanlis.transform.localScale = Vector3.one;
            yanlis.transform.localRotation = Quaternion.identity;
        }

        scorePanel.transform.SetAsLastSibling();

        if (scoreManager != null) EditorUtility.SetDirty(scoreManager);
    }

    private static void ReorganizeNotificationPanel(GameObject panel, Text txt, GameObject canvas)
    {
        if (panel == null) return;
        panel.SetActive(false); // Hide by default as requested

        LayoutGroup lg = panel.GetComponent<LayoutGroup>();
        if (lg != null) DestroyImmediate(lg);

        panel.transform.SetParent(canvas.transform, false);

        RectTransform r = panel.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0.5f, 0.5f);
        r.anchorMax = new Vector2(0.5f, 0.5f);
        r.pivot = new Vector2(0.5f, 0.5f);
        r.anchoredPosition = new Vector2(0f, 480f); // Position in upper-middle
        r.sizeDelta = new Vector2(900f, 120f); // Size: 900x120

        Image img = panel.GetComponent<Image>();
        if (img == null) img = panel.AddComponent<Image>();
        img.color = new Color32(15, 23, 42, 220); // Semi-transparent slate 900

        if (txt != null)
        {
            txt.transform.SetParent(panel.transform, false);
            RectTransform tr = txt.GetComponent<RectTransform>();
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.offsetMin = new Vector2(20f, 10f);
            tr.offsetMax = new Vector2(-20f, -10f);

            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontSize = 30;
            txt.color = Color.white;
            txt.transform.localScale = Vector3.one;
            txt.transform.localRotation = Quaternion.identity;
        }
    }

    private static void ReorganizeDecisionPanel(GameObject panel, TextMeshProUGUI gecmis)
    {
        if (panel == null) return;

        LayoutGroup lg = panel.GetComponent<LayoutGroup>();
        if (lg != null) DestroyImmediate(lg);

        RectTransform r = panel.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0.5f, 0f);
        r.anchorMax = new Vector2(0.5f, 0f);
        r.pivot = new Vector2(0.5f, 0f);
        r.anchoredPosition = new Vector2(0f, 50f); // Pos Y: 50
        r.sizeDelta = new Vector2(1600f, 520f); // Width: 1600, Height: 520

        Image img = panel.GetComponent<Image>();
        if (img == null) img = panel.AddComponent<Image>();
        img.color = new Color32(15, 23, 42, 220); // Semi-transparent slate 900

        // İlaç Geçmişi Yazısı (Alt-orta bölge)
        if (gecmis != null)
        {
            gecmis.transform.SetParent(panel.transform, false);
            RectTransform tr = gecmis.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0.05f, 0.35f);
            tr.anchorMax = new Vector2(0.95f, 0.49f);
            tr.pivot = new Vector2(0.5f, 0f);
            tr.anchoredPosition = Vector2.zero;
            tr.sizeDelta = Vector2.zero;

            gecmis.alignment = TextAlignmentOptions.Center;
            gecmis.fontSize = 24;
            gecmis.color = new Color32(148, 163, 184, 255); // Slate 400
            gecmis.transform.localScale = Vector3.one;
            gecmis.transform.localRotation = Quaternion.identity;
        }
    }

    private static void ReorganizeDecisionButtons(GameObject panel, Button btnA, Button btnB)
    {
        // Buton A (Hemen İlaç Al - Yeşil) - KararPanel içerisine taşınır
        if (btnA != null)
        {
            LayoutGroup lg = btnA.GetComponent<LayoutGroup>();
            if (lg != null) DestroyImmediate(lg);

            btnA.transform.SetParent(panel.transform, false);
            RectTransform tr = btnA.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0.5f, 0f);
            tr.anchorMax = new Vector2(0.5f, 0f);
            tr.pivot = new Vector2(0.5f, 0f);
            tr.anchoredPosition = new Vector2(-380f, 40f); // X: -380, Y: 40
            tr.sizeDelta = new Vector2(650f, 110f); // Width: 650, Height: 110

            Image bImg = btnA.GetComponent<Image>();
            if (bImg == null) bImg = btnA.gameObject.AddComponent<Image>();
            bImg.color = new Color32(16, 185, 129, 255); // Emerald green

            StyleButtonText(btnA, "Hemen ilaç al +5");
            btnA.transform.localScale = Vector3.one;
            btnA.transform.localRotation = Quaternion.identity;
        }

        // Buton B (Sonra Alırım - Kırmızı) - KararPanel içerisine taşınır
        if (btnB != null)
        {
            LayoutGroup lg = btnB.GetComponent<LayoutGroup>();
            if (lg != null) DestroyImmediate(lg);

            btnB.transform.SetParent(panel.transform, false);
            RectTransform tr = btnB.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0.5f, 0f);
            tr.anchorMax = new Vector2(0.5f, 0f);
            tr.pivot = new Vector2(0.5f, 0f);
            tr.anchoredPosition = new Vector2(380f, 40f); // X: 380, Y: 40
            tr.sizeDelta = new Vector2(650f, 110f); // Width: 650, Height: 110

            Image bImg = btnB.GetComponent<Image>();
            if (bImg == null) bImg = btnB.gameObject.AddComponent<Image>();
            bImg.color = new Color32(239, 68, 68, 255); // Rose Red

            StyleButtonText(btnB, "Sonra alırım -5");
            btnB.transform.localScale = Vector3.one;
            btnB.transform.localRotation = Quaternion.identity;
        }
    }

    private static void CreateDashboardReturnButton(GameObject canvas)
    {
        // Sahnede varsa silip temiz oluştur
        GameObject existing = GameObject.Find(canvas.name + "/AnaPaneleDonButonu");
        if (existing != null) DestroyImmediate(existing);

        GameObject btnGO = new GameObject("AnaPaneleDonButonu");
        btnGO.transform.SetParent(canvas.transform, false);

        RectTransform tr = btnGO.AddComponent<RectTransform>();
        tr.anchorMin = new Vector2(0f, 1f);
        tr.anchorMax = new Vector2(0f, 1f);
        tr.pivot = new Vector2(0f, 1f);
        tr.anchoredPosition = new Vector2(30f, -25f); // Positioned nicely at Y = -25
        tr.sizeDelta = new Vector2(260f, 70f); // Size: 260x70

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color32(15, 23, 42, 220); // Semi-transparent slate 900

        btnGO.AddComponent<Button>();

        // Yazı ekle (TMP)
        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);

        RectTransform txtRect = txtGO.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = txtGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "Ana Panele Dön";
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;
        tmp.transform.localScale = Vector3.one;
        tmp.transform.localRotation = Quaternion.identity;
    }

    private static void StyleButtonText(Button btn, string label)
    {
        if (btn == null) return;
        Text t = btn.GetComponentInChildren<Text>();
        if (t != null)
        {
            t.text = label;
            t.fontSize = 30;
            t.alignment = TextAnchor.MiddleCenter;
            t.color = Color.white;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;

            RectTransform tr = t.GetComponent<RectTransform>();
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.offsetMin = Vector2.zero;
            tr.offsetMax = Vector2.zero;
            t.transform.localScale = Vector3.one;
            t.transform.localRotation = Quaternion.identity;
        }
        TextMeshProUGUI tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = label;
            tmp.fontSize = 30;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.fontStyle = FontStyles.Bold;

            RectTransform tr = tmp.GetComponent<RectTransform>();
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.offsetMin = Vector2.zero;
            tr.offsetMax = Vector2.zero;
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localRotation = Quaternion.identity;
        }
    }

    private static GameObject CreatePanel(GameObject parent, string name, Color bgColor)
    {
        // Eski panel varsa temizle (Çakışmaları önlemek için)
        Transform existing = parent.transform.Find(name);
        if (existing != null)
        {
            DestroyImmediate(existing.gameObject);
        }

        GameObject panelGO = new GameObject(name);
        panelGO.transform.SetParent(parent.transform, false);

        RectTransform rect = panelGO.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        if (bgColor.a > 0)
        {
            Image img = panelGO.AddComponent<Image>();
            img.color = bgColor;
        }

        return panelGO;
    }

    private static void PopulateSplashScreen(GameObject splash)
    {
        // Ana Başlık
        CreateText(splash, "Baslik", "APPLİVER", 80, TextAlignmentOptions.Center,
            new Vector2(0.1f, 0.6f), new Vector2(0.9f, 0.8f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Canlı Mint Yeşili

        // Alt Başlık
        CreateText(splash, "AltBaslik", "Karaciğer Nakli Sonrası\nHasta Takip ve Destek Sistemi", 36, TextAlignmentOptions.Center,
            new Vector2(0.1f, 0.45f), new Vector2(0.9f, 0.6f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(148, 163, 184, 255)); // Yumuşak Slate Grisi

        // Başla Butonu
        CreateButton(splash, "BaslaButonu", "TAKİBE BAŞLA",
            new Vector2(0.5f, 0.2f), new Vector2(0.5f, 0.2f), new Vector2(0.5f, 0.5f), new Vector2(0, 0),
            new Vector2(500, 120), new Color32(2, 132, 199, 255)); // Koyu Mavi
    }

    private static void PopulatePatientInfoScreen(GameObject patientInfo)
    {
        // Ekran Başlığı (Move slightly lower, reduce font size to prevent top clipping)
        CreateText(patientInfo, "Baslik", "HAMZA'NIN HİKAYESİ", 48, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255));

        // Kart 1: Nakil Süreci (Y-anchor: 0.64 to 0.79)
        CreateCardWithTitleAndText(patientInfo, "NakilKarti", "1. Nakil Süreci",
            "Hamza, yakın zamanda başarılı bir karaciğer nakli operasyonu geçirdi. Yeni organının sağlıklı çalışması günlük bakımına bağlıdır.",
            new Vector2(0.08f, 0.64f), new Vector2(0.92f, 0.79f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // Kart 2: İlaç Takibi (Y-anchor: 0.47 to 0.62)
        CreateCardWithTitleAndText(patientInfo, "TakipKarti", "2. İlaç Takibi",
            "Bağışıklık sisteminin yeni karaciğeri reddetmemesi için koruyucu ilaçların her gün tam zamanında ve eksiksiz alınması kritik önem taşır.",
            new Vector2(0.08f, 0.47f), new Vector2(0.92f, 0.62f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // Kart 3: Organ Reddi Riski (Y-anchor: 0.30 to 0.45)
        CreateCardWithTitleAndText(patientInfo, "RiskKarti", "3. Organ Reddi Riski",
            "İlaç dozlarının kaçırılması veya geciktirilmesi organ reddi reaksiyonunu tetikleyebilir. Bu durum hayati tehlike oluşturur!",
            new Vector2(0.08f, 0.30f), new Vector2(0.92f, 0.45f),
            new Color32(239, 68, 68, 255)); // Vivid Red

        // Replay Butonu (SpeakerButton) - Shifted to Y: 0.24, Size: 380x75
        CreateButton(patientInfo, "SpeakerButton", "🔊 Dinle",
            new Vector2(0.5f, 0.24f), new Vector2(0.5f, 0.24f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(380, 75), new Color32(71, 85, 105, 255)); // Koyu Slate

        // İleri Butonu (DashboardButonu) - Shifted to Y: 0.15, Size: 580x100
        CreateButton(patientInfo, "DashboardButonu", "KONTROL PANELİNE GİT",
            new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(580, 100), new Color32(14, 165, 233, 255)); // Modern Medikal Mavi

        // Audio support setup
        PatientStoryAudioController audioController = patientInfo.GetComponent<PatientStoryAudioController>();
        if (audioController == null)
        {
            audioController = patientInfo.AddComponent<PatientStoryAudioController>();
        }

        AudioSource audioSource = patientInfo.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/hamza_story.wav");
            if (clip != null)
            {
                audioSource.clip = clip;
                Debug.Log("Appliver - hamza_story.wav successfully assigned to PatientInfoScreen AudioSource.");
            }
            else
            {
                Debug.LogWarning("Appliver - Assets/Audio/hamza_story.wav not found! Please ensure the file exists.");
            }
        }

        // Back Button (AnaSayfayaDonButonu)
        GameObject btnGO = new GameObject("AnaSayfayaDonButonu");
        btnGO.transform.SetParent(patientInfo.transform, false);
        RectTransform rect = btnGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(25f, -25f);
        rect.sizeDelta = new Vector2(220f, 60f);

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color32(30, 41, 59, 255); // Dark Slate 800
        Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSprite != null)
        {
            img.sprite = roundedSprite;
            img.type = Image.Type.Sliced;
        }

        btnGO.AddComponent<Button>();

        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);
        RectTransform txtRect = txtGO.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = txtGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "← ANA SAYFAYA DÖN";
        tmp.fontSize = 20;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;
    }

    private static void PopulateDashboardScreen(GameObject dashboard)
    {
        // Ekran Başlığı (Merhaba Hamza 👋)
        CreateText(dashboard, "Baslik", "Merhaba Hamza 👋", 48, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Canlı Mint Yeşili

        // Alt Başlık (Bugünkü takip durumun)
        CreateText(dashboard, "AltBaslik", "Bugünkü takip durumun", 28, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.76f), new Vector2(0.95f, 0.82f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(148, 163, 184, 255)); // Slate 400

        // Takip Özet Kartı
        string durumOzeti = "Sağlık Durumu: İYİ\nRisk Durumu: DÜŞÜK\nSon İlaç: 21:30";
        CreateCardWithTitleAndText(dashboard, "DurumKarti", "Takip Özetiniz", durumOzeti,
            new Vector2(0.08f, 0.54f), new Vector2(0.92f, 0.72f),
            new Color32(0, 245, 196, 255)); // Mint Green

        // 1. ARButonu (AR SİMÜLASYON) - Center Y: 0.43, Size: 650x75, Color: Medical Blue (#0EA5E9)
        CreateButton(dashboard, "ARButonu", "AR SİMÜLASYON",
            new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(14, 165, 233, 255)); // Medical Blue

        // 2. DailyButonu (GÜNLÜK GÖREVLER) - Center Y: 0.34, Size: 650x75, Color: Emerald Green (#10B981)
        CreateButton(dashboard, "DailyButonu", "GÜNLÜK GÖREVLER",
            new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.34f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(16, 185, 129, 255)); // Emerald Green

        // 3. DoctorHistoryButonu (DOKTOR GEÇMİŞİ) - Center Y: 0.25, Size: 650x75, Color: Slate Gray (#475569)
        CreateButton(dashboard, "DoctorHistoryButonu", "DOKTOR GEÇMİŞİ",
            new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.25f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(71, 85, 105, 255)); // Slate Gray

        // 4. HikayeButonu (HİKAYEYİ TEKRAR OKU) - Center Y: 0.16, Size: 650x75, Color: Purple / Indigo (#6366F1)
        CreateButton(dashboard, "HikayeButonu", "HİKAYEYİ TEKRAR OKU",
            new Vector2(0.5f, 0.16f), new Vector2(0.5f, 0.16f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(650, 75), new Color32(99, 102, 241, 255)); // Purple / Indigo
    }

    private static void PopulateARSimulationScreen(GameObject arSim)
    {
        // Üst Bilgilendirme Paneli
        GameObject topBar = CreatePanel(arSim, "TopPanel", new Color32(15, 23, 42, 200)); // Yarı transparan koyu arka plan
        RectTransform barRect = topBar.GetComponent<RectTransform>();
        barRect.anchorMin = new Vector2(0.0f, 0.82f);
        barRect.anchorMax = new Vector2(1.0f, 1.0f);
        barRect.offsetMin = Vector2.zero;
        barRect.offsetMax = Vector2.zero;

        CreateText(topBar, "BilgiMetni", "AR Karaciğer Simülasyonu", 32, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.1f), new Vector2(0.95f, 0.9f), new Vector2(0.5f, 0.5f), Vector2.zero,
            Color.white);

        // Geri Dön Butonu (Ekranın sol üstünde asılı durur)
        CreateButton(arSim, "GeriButonu", "GERİ DÖN",
            new Vector2(0.5f, 0.08f), new Vector2(0.5f, 0.08f), new Vector2(0.5f, 0.5f), new Vector2(0, 0),
            new Vector2(400, 100), new Color32(239, 68, 68, 255)); // Yumuşak Kırmızı
    }

    private static void PopulateDailyCareScreen(GameObject dailyCare)
    {
        // Background color of DailyCareScreen set to premium dark navy
        Image bgImage = dailyCare.GetComponent<Image>();
        if (bgImage == null) bgImage = dailyCare.AddComponent<Image>();
        bgImage.color = new Color32(10, 15, 30, 255); // Premium Deep Navy

        // Ekran Başlığı (Baslik): GÜNLÜK BAKIM GÖREVLERİ
        CreateText(dailyCare, "Baslik", "GÜNLÜK BAKIM GÖREVLERİ", 44, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Mint Green

        // Görev Listesi Kartı (GorevKarti)
        GameObject listCard = CreatePanel(dailyCare, "GorevKarti", new Color32(30, 41, 59, 255)); // Slate 800
        Image cardImg = listCard.GetComponent<Image>();
        if (cardImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                cardImg.sprite = roundedSprite;
                cardImg.type = Image.Type.Sliced;
            }
        }
        RectTransform listRect = listCard.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0.08f, 0.26f);
        listRect.anchorMax = new Vector2(0.92f, 0.78f);
        listRect.offsetMin = Vector2.zero;
        listRect.offsetMax = Vector2.zero;

        // Build 5 separate interactive Toggles inside GorevKarti
        GameObject toggle0 = CreateToggleRow(listCard, "Toggle_0", "İlaç alındı", new Vector2(0.05f, 0.792f), new Vector2(0.95f, 0.92f));
        GameObject toggle1 = CreateToggleRow(listCard, "Toggle_1", "Su içildi", new Vector2(0.05f, 0.664f), new Vector2(0.95f, 0.792f));
        GameObject toggle2 = CreateToggleRow(listCard, "Toggle_2", "Hafif yürüyüş", new Vector2(0.05f, 0.536f), new Vector2(0.95f, 0.664f));
        GameObject toggle3 = CreateToggleRow(listCard, "Toggle_3", "Ateş kontrolü", new Vector2(0.05f, 0.408f), new Vector2(0.95f, 0.536f));
        GameObject toggle4 = CreateToggleRow(listCard, "Toggle_4", "Hijyen", new Vector2(0.05f, 0.28f), new Vector2(0.95f, 0.408f));

        // Progress Bar Container (ProgressContainer)
        GameObject progressContainer = CreatePanel(listCard, "ProgressContainer", new Color32(0, 0, 0, 0));
        RectTransform progressContainerRect = progressContainer.GetComponent<RectTransform>();
        progressContainerRect.anchorMin = new Vector2(0.08f, 0.05f);
        progressContainerRect.anchorMax = new Vector2(0.92f, 0.24f);
        progressContainerRect.offsetMin = Vector2.zero;
        progressContainerRect.offsetMax = Vector2.zero;

        // Progress Text: "Günlük Uyum: %0"
        TextMeshProUGUI progressTextTmp = CreateText(progressContainer, "ProgressText", "Günlük Uyum: %0", 26, TextAlignmentOptions.Left,
            new Vector2(0f, 0.55f), new Vector2(1f, 1.0f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(226, 232, 240, 255)); // Slate 200

        // Progress Track Background
        GameObject progressTrack = CreatePanel(progressContainer, "ProgressTrack", new Color32(15, 23, 42, 255)); // Deep Slate 900
        Image trackImg = progressTrack.GetComponent<Image>();
        if (trackImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                trackImg.sprite = roundedSprite;
                trackImg.type = Image.Type.Sliced;
            }
        }
        RectTransform trackRect = progressTrack.GetComponent<RectTransform>();
        trackRect.anchorMin = new Vector2(0f, 0.15f);
        trackRect.anchorMax = new Vector2(1f, 0.45f);
        trackRect.offsetMin = Vector2.zero;
        trackRect.offsetMax = Vector2.zero;

        // Progress Fill Bar (Mint Green, Filled type for dynamic updates)
        GameObject progressFill = CreatePanel(progressTrack, "ProgressFill", new Color32(0, 245, 196, 255)); // Mint Green
        Image fillImg = progressFill.GetComponent<Image>();
        if (fillImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                fillImg.sprite = roundedSprite;
            }
            fillImg.type = Image.Type.Filled;
            fillImg.fillMethod = Image.FillMethod.Horizontal;
            fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImg.fillAmount = 0.6f;
        }
        RectTransform fillRect = progressFill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        // Geri Butonu (GeriButonu) - Shifted Y to 0.18 for better visibility, Size: 500x90, Azure Blue (#0EA5E9)
        CreateButton(dailyCare, "GeriButonu", "GERİ",
            new Vector2(0.5f, 0.18f), new Vector2(0.5f, 0.18f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(500, 90), new Color32(14, 165, 233, 255));

        // 5. Setup controller and assign references
        DailyCareController controller = dailyCare.GetComponent<DailyCareController>();
        if (controller == null) controller = dailyCare.AddComponent<DailyCareController>();

        controller.toggles = new Toggle[5] {
            toggle0.GetComponent<Toggle>(),
            toggle1.GetComponent<Toggle>(),
            toggle2.GetComponent<Toggle>(),
            toggle3.GetComponent<Toggle>(),
            toggle4.GetComponent<Toggle>()
        };
        controller.progressText = progressTextTmp;
        controller.progressFillImage = fillImg;

        EditorUtility.SetDirty(controller);
    }

    private static TextMeshProUGUI CreateText(GameObject parent, string name, string textContent, float fontSize, TextAlignmentOptions alignment,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos, Color color)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(parent.transform, false);

        RectTransform rect = textGO.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = Vector2.zero; // Stretch modda sıfır yap

        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = textContent;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.color = color;
        return tmp;
    }

    private static GameObject CreateCardWithTitleAndText(GameObject parent, string cardName, string title, string body, Vector2 anchorMin, Vector2 anchorMax, Color32 titleColor)
    {
        // 1. Card Panel
        GameObject card = CreatePanel(parent, cardName, new Color32(30, 41, 59, 240)); // Semi-transparent Slate
        Image img = card.GetComponent<Image>();
        if (img != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                img.sprite = roundedSprite;
                img.type = Image.Type.Sliced;
            }
        }

        RectTransform cardRect = card.GetComponent<RectTransform>();
        cardRect.anchorMin = anchorMin;
        cardRect.anchorMax = anchorMax;
        cardRect.offsetMin = Vector2.zero;
        cardRect.offsetMax = Vector2.zero;

        // 2. Title Text
        TextMeshProUGUI titleTmp = CreateText(card, "Baslik", title, 32, TextAlignmentOptions.MidlineLeft,
            new Vector2(0.06f, 0.68f), new Vector2(0.94f, 0.90f), new Vector2(0f, 1f), Vector2.zero,
            titleColor);
        titleTmp.enableWordWrapping = true;
        titleTmp.fontStyle = FontStyles.Bold;

        // 3. Body Text
        TextMeshProUGUI bodyTmp = CreateText(card, "Metin", body, 25, TextAlignmentOptions.TopLeft,
            new Vector2(0.06f, 0.08f), new Vector2(0.94f, 0.64f), new Vector2(0f, 1f), Vector2.zero,
            new Color32(226, 232, 240, 255)); // Slate 200
        bodyTmp.enableWordWrapping = true;
        bodyTmp.enableAutoSizing = true;
        bodyTmp.fontSizeMin = 18;
        bodyTmp.fontSizeMax = 25;

        return card;
    }

    private static void CreateButton(GameObject parent, string name, string label,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos, Vector2 size, Color32 buttonColor)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent.transform, false);

        RectTransform rect = btnGO.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = size;

        Image img = btnGO.AddComponent<Image>();
        img.color = buttonColor;

        // Use Unity's built-in rounded UISprite for a modern pill look
        Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSprite != null)
        {
            img.sprite = roundedSprite;
            img.type = Image.Type.Sliced;
        }

        Button btn = btnGO.AddComponent<Button>();

        // Buton içindeki yazı objesi
        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);

        RectTransform txtRect = txtGO.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = txtGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 28;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontStyle = FontStyles.Bold;
    }

    private static Transform FindRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform found = FindRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }

    private static GameObject CreateToggleRow(GameObject parent, string name, string labelText, Vector2 anchorMin, Vector2 anchorMax)
    {
        // 1. Row Container
        GameObject rowGO = new GameObject(name);
        rowGO.transform.SetParent(parent.transform, false);

        RectTransform rowRect = rowGO.AddComponent<RectTransform>();
        rowRect.anchorMin = anchorMin;
        rowRect.anchorMax = anchorMax;
        rowRect.offsetMin = Vector2.zero;
        rowRect.offsetMax = Vector2.zero;

        Toggle toggle = rowGO.AddComponent<Toggle>();

        // 2. Checkbox Background (The Box)
        GameObject bgGO = new GameObject("Background");
        bgGO.transform.SetParent(rowGO.transform, false);

        RectTransform bgRect = bgGO.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0.5f);
        bgRect.anchorMax = new Vector2(0f, 0.5f);
        bgRect.pivot = new Vector2(0f, 0.5f);
        bgRect.anchoredPosition = new Vector2(30f, 0f);
        bgRect.sizeDelta = new Vector2(50f, 50f);

        Image bgImage = bgGO.AddComponent<Image>();
        bgImage.color = new Color32(15, 23, 42, 255); // Dark Slate 900
        Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSprite != null)
        {
            bgImage.sprite = roundedSprite;
            bgImage.type = Image.Type.Sliced;
        }

        // 3. Checkmark (Vibrant Mint Green check inside the box)
        GameObject checkGO = new GameObject("Checkmark");
        checkGO.transform.SetParent(bgGO.transform, false);

        RectTransform checkRect = checkGO.AddComponent<RectTransform>();
        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.offsetMin = new Vector2(8f, 8f);
        checkRect.offsetMax = new Vector2(-8f, -8f);

        Image checkImage = checkGO.AddComponent<Image>();
        checkImage.color = new Color32(0, 245, 196, 255); // Mint Green
        Sprite checkSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        if (checkSprite != null)
        {
            checkImage.sprite = checkSprite;
        }

        // 4. Label Text
        GameObject labelGO = new GameObject("Label");
        labelGO.transform.SetParent(rowGO.transform, false);

        RectTransform labelRect = labelGO.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0f, 0f);
        labelRect.anchorMax = new Vector2(1f, 1f);
        labelRect.offsetMin = new Vector2(100f, 0f); // Spaced from the checkbox
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI labelTmp = labelGO.AddComponent<TextMeshProUGUI>();
        labelTmp.text = labelText;
        labelTmp.fontSize = 32;
        labelTmp.alignment = TextAlignmentOptions.MidlineLeft;
        labelTmp.color = new Color32(226, 232, 240, 255); // Slate 200

        // Wire Toggle
        toggle.targetGraphic = bgImage;
        toggle.graphic = checkImage;

        return rowGO;
    }

    private static void PopulateDoctorHistoryScreen(GameObject doctorHistory)
    {
        // 1. Title: DOKTOR GEÇMİŞİ
        CreateText(doctorHistory, "Baslik", "DOKTOR GEÇMİŞİ", 44, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.90f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Mint Green

        // 2. Timeline Cards (As Buttons for clickability)
        GameObject card0 = CreateCardButton(doctorHistory, "Card_0", new Vector2(0.08f, 0.62f), new Vector2(0.92f, 0.76f));
        GameObject card1 = CreateCardButton(doctorHistory, "Card_1", new Vector2(0.08f, 0.45f), new Vector2(0.92f, 0.59f));
        GameObject card2 = CreateCardButton(doctorHistory, "Card_2", new Vector2(0.08f, 0.28f), new Vector2(0.92f, 0.42f));

        // 3. Popup Panel (Modal Popup)
        GameObject popupPanel = CreatePanel(doctorHistory, "PopupPanel", new Color32(15, 23, 42, 245)); // Deep slate
        Image popupImg = popupPanel.GetComponent<Image>();
        if (popupImg != null)
        {
            Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            if (roundedSprite != null)
            {
                popupImg.sprite = roundedSprite;
                popupImg.type = Image.Type.Sliced;
            }
        }
        RectTransform popupRect = popupPanel.GetComponent<RectTransform>();
        popupRect.anchorMin = new Vector2(0.12f, 0.35f);
        popupRect.anchorMax = new Vector2(0.88f, 0.65f);
        popupRect.offsetMin = Vector2.zero;
        popupRect.offsetMax = Vector2.zero;

        // Popup Title Text
        TextMeshProUGUI popupTitle = CreateText(popupPanel, "PopupTitle", "Doktor Notu", 36, TextAlignmentOptions.Center,
            new Vector2(0.05f, 0.72f), new Vector2(0.95f, 0.88f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(0, 245, 196, 255)); // Mint Green
        popupTitle.fontStyle = FontStyles.Bold;

        // Popup Note Text
        TextMeshProUGUI popupNote = CreateText(popupPanel, "PopupNote", "Not icerigi", 28, TextAlignmentOptions.Center,
            new Vector2(0.08f, 0.22f), new Vector2(0.92f, 0.68f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Color32(226, 232, 240, 255)); // Slate 200
        popupNote.enableWordWrapping = true;
        popupNote.enableAutoSizing = true;
        popupNote.fontSizeMin = 20;
        popupNote.fontSizeMax = 28;

        // Popup Close Button (X)
        GameObject closeBtnGO = new GameObject("PopupCloseButton");
        closeBtnGO.transform.SetParent(popupPanel.transform, false);
        RectTransform closeBtnRect = closeBtnGO.AddComponent<RectTransform>();
        closeBtnRect.anchorMin = new Vector2(0.86f, 0.78f);
        closeBtnRect.anchorMax = new Vector2(0.95f, 0.93f);
        closeBtnRect.offsetMin = Vector2.zero;
        closeBtnRect.offsetMax = Vector2.zero;
        Image closeImg = closeBtnGO.AddComponent<Image>();
        closeImg.color = new Color32(239, 68, 68, 255); // Red
        Sprite roundedSpriteForClose = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSpriteForClose != null)
        {
            closeImg.sprite = roundedSpriteForClose;
            closeImg.type = Image.Type.Sliced;
        }
        closeBtnGO.AddComponent<Button>();

        GameObject closeTxtGO = new GameObject("Text");
        closeTxtGO.transform.SetParent(closeBtnGO.transform, false);
        RectTransform closeTxtRect = closeTxtGO.AddComponent<RectTransform>();
        closeTxtRect.anchorMin = Vector2.zero;
        closeTxtRect.anchorMax = Vector2.one;
        closeTxtRect.offsetMin = Vector2.zero;
        closeTxtRect.offsetMax = Vector2.zero;
        TextMeshProUGUI closeTxtTmp = closeTxtGO.AddComponent<TextMeshProUGUI>();
        closeTxtTmp.text = "X";
        closeTxtTmp.fontSize = 24;
        closeTxtTmp.alignment = TextAlignmentOptions.Center;
        closeTxtTmp.color = Color.white;
        closeTxtTmp.fontStyle = FontStyles.Bold;

        // 4. Return Button (GeriButonu)
        CreateButton(doctorHistory, "GeriButonu", "GERİ",
            new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.15f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(500, 90), new Color32(14, 165, 233, 255)); // Azure Blue

        // 5. Bind DoctorHistoryController
        DoctorHistoryController controller = doctorHistory.GetComponent<DoctorHistoryController>();
        if (controller == null) controller = doctorHistory.AddComponent<DoctorHistoryController>();

        controller.cardButtons = new Button[3] {
            card0.GetComponent<Button>(),
            card1.GetComponent<Button>(),
            card2.GetComponent<Button>()
        };
        controller.popupPanel = popupPanel;
        controller.popupTitleText = popupTitle;
        controller.popupNoteText = popupNote;
        controller.popupCloseButton = closeBtnGO.GetComponent<Button>();

        // Close popup by default in Editor
        popupPanel.SetActive(false);

        EditorUtility.SetDirty(controller);
    }

    private static GameObject CreateCardButton(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject cardGO = new GameObject(name);
        cardGO.transform.SetParent(parent.transform, false);

        RectTransform rect = cardGO.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image img = cardGO.AddComponent<Image>();
        img.color = new Color32(30, 41, 59, 255); // Slate 800
        Sprite roundedSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (roundedSprite != null)
        {
            img.sprite = roundedSprite;
            img.type = Image.Type.Sliced;
        }

        cardGO.AddComponent<Button>();

        // Text inside card
        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(cardGO.transform, false);
        RectTransform txtRect = txtGO.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = new Vector2(40f, 20f);
        txtRect.offsetMax = new Vector2(-40f, -20f);

        TextMeshProUGUI tmp = txtGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.fontSize = 28;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.color = Color.white;
        tmp.enableWordWrapping = true;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 20;
        tmp.fontSizeMax = 32;

        return cardGO;
    }
}