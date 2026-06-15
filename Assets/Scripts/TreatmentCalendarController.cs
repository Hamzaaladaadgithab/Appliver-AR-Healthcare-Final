using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreatmentCalendarController : MonoBehaviour
{
    [Header("Durum")]
    public TextMeshProUGUI statusText;

    private const float CardWidth = 930f;
    private const float ButtonHeight = 92f;

    private AppNavigationManager navigationManager;

    public static GameObject EnsureScreen(Transform appScreensCanvas, AppNavigationManager navManager)
    {
        if (appScreensCanvas == null) return null;

        Transform existing = appScreensCanvas.Find("TreatmentCalendarScreen");
        GameObject screen = existing != null
            ? existing.gameObject
            : CreateScreen(appScreensCanvas);

        TreatmentCalendarController controller = screen.GetComponent<TreatmentCalendarController>();
        if (controller == null)
        {
            controller = screen.AddComponent<TreatmentCalendarController>();
        }

        controller.navigationManager = navManager;
        controller.BindButtons();
        screen.SetActive(false);
        return screen;
    }

    public void MarkAsCompleted()
    {
        SetStatus("\u0130la\u00e7 al\u0131m\u0131 kaydedildi.");
    }

    public void RemindLater()
    {
        SetStatus("Hat\u0131rlatma daha sonra tekrar g\u00f6sterilecek.");
    }

    public void MarkDoseMissed()
    {
        SetStatus("Doz ka\u00e7\u0131r\u0131ld\u0131ysa doktorunuzun \u00f6nerdi\u011fi talimatlar\u0131 takip ediniz.");
    }

    public void AddTreatmentItem()
    {
    }

    public void UpdateTreatmentItem()
    {
    }

    public void DeleteTreatmentItem()
    {
    }

    private void BindButtons()
    {
        BindButton("IlacAlindiButonu", MarkAsCompleted);
        BindButton("DahaSonraHatirlatButonu", RemindLater);
        BindButton("DozKacirildiButonu", MarkDoseMissed);
        BindButton("AREgitimineGitButonu", GoToAREducation);
        BindButton("BackArrowButonu", GoBack);

        if (statusText == null)
        {
            Transform status = FindRecursive(transform, "StatusText");
            if (status != null)
            {
                statusText = status.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private void BindButton(string buttonName, UnityEngine.Events.UnityAction action)
    {
        Transform buttonTransform = FindRecursive(transform, buttonName);
        if (buttonTransform == null) return;

        Button button = buttonTransform.GetComponent<Button>();
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    private void GoToAREducation()
    {
        navigationManager?.ShowARSimulation();
    }

    private void GoBack()
    {
        navigationManager?.ShowDashboard();
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private static GameObject CreateScreen(Transform parent)
    {
        GameObject screen = CreateUIObject("TreatmentCalendarScreen", parent);
        Image background = screen.AddComponent<Image>();
        background.color = new Color32(10, 15, 30, 255);

        RectTransform screenRect = screen.GetComponent<RectTransform>();
        screenRect.anchorMin = Vector2.zero;
        screenRect.anchorMax = Vector2.one;
        screenRect.offsetMin = Vector2.zero;
        screenRect.offsetMax = Vector2.zero;

        ScrollRect scrollRect = screen.AddComponent<ScrollRect>();
        GameObject viewport = CreateUIObject("Viewport", screen.transform);
        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.color = Color.white;
        Mask viewportMask = viewport.AddComponent<Mask>();
        viewportMask.showMaskGraphic = false;

        RectTransform viewportRect = viewport.GetComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(50f, 54f);
        viewportRect.offsetMax = new Vector2(-50f, -150f);

        GameObject content = CreateUIObject("Content", viewport.transform);
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = Vector2.zero;

        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(0, 0, 28, 52);
        layout.spacing = 26f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;

        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scrollRect.viewport = viewportRect;
        scrollRect.content = contentRect;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        scrollRect.elasticity = 0.08f;
        scrollRect.inertia = true;
        scrollRect.decelerationRate = 0.16f;
        scrollRect.scrollSensitivity = 18f;

        CreateSectionCard(content.transform, "Card_BugunkuTedaviPlani", "Bug\u00fcnk\u00fc Tedavi Plan\u0131",
            "09:00 - \u0130la\u00e7 hat\u0131rlatmas\u0131\n13:00 - Su t\u00fcketimi kontrol\u00fc\n18:00 - Ate\u015f \u00f6l\u00e7\u00fcm\u00fc");
        CreateSectionCard(content.transform, "Card_YaklasanKontroller", "Yakla\u015fan Kontroller",
            "10 Haziran 2026 - Kan testi\n15 Haziran 2026 - Doktor kontrol\u00fc\n30 Haziran 2026 - \u0130la\u00e7 dozu de\u011ferlendirmesi");
        CreateSectionCard(content.transform, "Card_GecmisDoktorKayitlari", "Ge\u00e7mi\u015f Doktor Kay\u0131tlar\u0131",
            "27 May\u0131s 2026 - Kan testi tamamland\u0131\n30 May\u0131s 2026 - Kontrol muayenesi yap\u0131ld\u0131\n05 Haziran 2026 - \u0130la\u00e7 dozu g\u00fcncellendi");

        TextMeshProUGUI status = CreateInteractionCard(content.transform);

        CreateHeader(screen.transform);

        TreatmentCalendarController controller = screen.AddComponent<TreatmentCalendarController>();
        controller.statusText = status;
        return screen;
    }

    private static void CreateHeader(Transform parent)
    {
        GameObject header = CreateUIObject("Header", parent);
        RectTransform headerRect = header.GetComponent<RectTransform>();
        headerRect.anchorMin = new Vector2(0f, 1f);
        headerRect.anchorMax = new Vector2(1f, 1f);
        headerRect.pivot = new Vector2(0.5f, 1f);
        headerRect.offsetMin = new Vector2(0f, -128f);
        headerRect.offsetMax = Vector2.zero;

        GameObject titleObject = CreateUIObject("Title", header.transform);
        RectTransform titleRect = titleObject.GetComponent<RectTransform>();
        titleRect.anchorMin = Vector2.zero;
        titleRect.anchorMax = Vector2.one;
        titleRect.offsetMin = new Vector2(140f, 0f);
        titleRect.offsetMax = new Vector2(-140f, 0f);

        TextMeshProUGUI title = titleObject.AddComponent<TextMeshProUGUI>();
        title.text = "Tedavi Takvimi";
        title.fontSize = 44f;
        title.color = new Color32(0, 245, 196, 255);
        title.fontStyle = FontStyles.Bold;
        title.alignment = TextAlignmentOptions.Center;
        title.enableWordWrapping = false;

        GameObject backButton = CreateUIObject("BackArrowButonu", header.transform);
        RectTransform backRect = backButton.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0f, 1f);
        backRect.anchorMax = new Vector2(0f, 1f);
        backRect.pivot = new Vector2(0f, 1f);
        backRect.anchoredPosition = new Vector2(250f, -120f);
        backRect.sizeDelta = new Vector2(150f, 100f);
        backRect.localScale = Vector3.one;
        backRect.localRotation = Quaternion.identity;

        Image backImage = backButton.AddComponent<Image>();
        backImage.color = new Color32(30, 41, 59, 255);

        Button back = backButton.AddComponent<Button>();
        back.targetGraphic = backImage;

        TextMeshProUGUI arrow = CreateFixedText(backButton.transform, "Text", "\u2190", 52f, new Color32(0, 245, 196, 255), FontStyles.Bold);
        arrow.alignment = TextAlignmentOptions.Center;

        header.transform.SetAsLastSibling();
    }

    private static void CreateSectionCard(Transform parent, string objectName, string title, string body)
    {
        GameObject card = CreateCard(parent, objectName, 316f);

        TextMeshProUGUI titleText = CreateLayoutText(card.transform, "Baslik", title, 34f, new Color32(0, 245, 196, 255), FontStyles.Bold, 54f);
        titleText.alignment = TextAlignmentOptions.Left;

        TextMeshProUGUI bodyText = CreateLayoutText(card.transform, "Metin", body, 29f, new Color32(226, 232, 240, 255), FontStyles.Normal, 178f);
        bodyText.alignment = TextAlignmentOptions.Left;
        bodyText.lineSpacing = 12f;
    }

    private static TextMeshProUGUI CreateInteractionCard(Transform parent)
    {
        GameObject card = CreateCard(parent, "Card_Etkilesim", 668f);

        TextMeshProUGUI titleText = CreateLayoutText(card.transform, "Baslik", "Etkile\u015fim", 34f, new Color32(0, 245, 196, 255), FontStyles.Bold, 54f);
        titleText.alignment = TextAlignmentOptions.Left;

        TextMeshProUGUI status = CreateLayoutText(card.transform, "StatusText", "Hen\u00fcz i\u015flem yap\u0131lmad\u0131.", 29f, new Color32(226, 232, 240, 255), FontStyles.Normal, 76f);
        status.alignment = TextAlignmentOptions.Center;

        CreateButton(card.transform, "IlacAlindiButonu", "\u0130la\u00e7 Al\u0131nd\u0131", new Color32(16, 185, 129, 255));
        CreateButton(card.transform, "DahaSonraHatirlatButonu", "Daha Sonra Hat\u0131rlat", new Color32(14, 165, 233, 255));
        CreateButton(card.transform, "DozKacirildiButonu", "Doz Ka\u00e7\u0131r\u0131ld\u0131", new Color32(185, 28, 28, 255));
        CreateButton(card.transform, "AREgitimineGitButonu", "AR E\u011fitimine Git", new Color32(99, 102, 241, 255));

        return status;
    }

    private static GameObject CreateCard(Transform parent, string name, float height)
    {
        GameObject card = CreateUIObject(name, parent);
        RectTransform cardRect = card.GetComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(CardWidth, height);

        Image image = card.AddComponent<Image>();
        image.color = new Color32(30, 41, 59, 245);

        VerticalLayoutGroup layout = card.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(34, 34, 32, 28);
        layout.spacing = 24f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        LayoutElement cardLayout = card.AddComponent<LayoutElement>();
        cardLayout.preferredWidth = CardWidth;
        cardLayout.preferredHeight = height;
        return card;
    }

    private static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    private static TextMeshProUGUI CreateLayoutText(Transform parent, string name, string text, float fontSize, Color color, FontStyles style, float height)
    {
        GameObject textObject = CreateUIObject(name, parent);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
        tmp.overflowMode = TextOverflowModes.Overflow;

        LayoutElement layout = textObject.AddComponent<LayoutElement>();
        layout.preferredHeight = height;
        return tmp;
    }

    private static void CreateButton(Transform parent, string name, string label, Color32 color)
    {
        GameObject buttonObject = CreateUIObject(name, parent);
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(790f, ButtonHeight);

        Image image = buttonObject.AddComponent<Image>();
        image.color = color;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        LayoutElement buttonLayout = buttonObject.AddComponent<LayoutElement>();
        buttonLayout.preferredWidth = 790f;
        buttonLayout.preferredHeight = ButtonHeight;

        TextMeshProUGUI text = CreateFixedText(buttonObject.transform, "Text", label, 30f, Color.white, FontStyles.Bold);
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = true;
    }

    private static TextMeshProUGUI CreateFixedText(Transform parent, string name, string text, float fontSize, Color color, FontStyles style)
    {
        GameObject textObject = CreateUIObject(name, parent);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.fontStyle = style;
        tmp.enableWordWrapping = false;
        return tmp;
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
}
