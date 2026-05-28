using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

// ============================================================
// ScenarioManager.cs
// AR Hasta Takip Sistemi
// ============================================================

public class ScenarioManager : MonoBehaviour
{
    [Header("Karaciger Objesi")]
    public GameObject liverObject;

    [Header("UI Referanslari")]
    public Text bildirimText;

    public TextMeshProUGUI gecmisText;

    public GameObject bildirimPanel;
    public GameObject kararPanel;

    public Button butonA;
    public Button butonB;

    [Header("Zamanlama")]
    public float ilacHatirlatmaSuresi = 5f;
    public float bildirimGostermeSuresi = 3f;
    public float fadeSuresi = 0.3f;

    [Header("Renkler")]
    public Color saglikliRenk =
        new Color(0.11f, 0.62f, 0.46f);

    public Color riskliRenk =
        new Color(0.89f, 0.29f, 0.29f);

    public Color varsayilanRenk =
        new Color(0.55f, 0.13f, 0.13f);

    public Color bildirimRenk =
        new Color(0.18f, 0.18f, 0.18f, 0.95f);

    private ScoreManager scoreManager;

    private bool kararBekleniyor = false;

    private Coroutine aktifFade;

    int kacirdi = 0;

    // ============================================================

    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

        RenkSifirla();

        HerSeyleriGizle();

        string kayitliTarih =
            PlayerPrefs.GetString(
                "SonIlac",
                "Henüz alýnmadý"
            );

        if (gecmisText != null)
        {
            gecmisText.text =
                "Son ilaç: " + kayitliTarih;
        }

        StartCoroutine(IlacHatirlatmaDongusu());
    }

    // ============================================================

    void HerSeyleriGizle()
    {
        if (kararPanel != null)
            kararPanel.SetActive(false);

        if (bildirimPanel != null)
            bildirimPanel.SetActive(false);

        if (bildirimText != null)
            bildirimText.gameObject.SetActive(false);

        ButonlariAktiflestir(false);
    }

    // ============================================================

    IEnumerator IlacHatirlatmaDongusu()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                ilacHatirlatmaSuresi
            );

            IlacVaktiBildirimi();

            kararBekleniyor = true;

            yield return new WaitUntil(
                () => !kararBekleniyor
            );

            yield return new WaitForSeconds(
                bildirimGostermeSuresi
            );

            SifirlaVeDevam();
        }
    }

    // ============================================================

    void IlacVaktiBildirimi()
    {
        BildirimGoster(
            "Ýlaç alma zamaný!\nNe yapmak istersiniz?",
            bildirimRenk
        );

        if (kararPanel != null)
            kararPanel.SetActive(true);

        ButonlariAktiflestir(true);
    }

    // ============================================================
    // ILAC AL
    // ============================================================

    public void IlacAl()
    {
        if (!kararBekleniyor) return;

        kararBekleniyor = false;

        LiverRengi(saglikliRenk);

        scoreManager?.PuanEkle(5);

        ButonlariAktiflestir(false);

        if (kararPanel != null)
            kararPanel.SetActive(false);

        string tarihSaat =
            DateTime.Now.ToString(
                "dd.MM.yyyy - HH:mm"
            );

        PlayerPrefs.SetString(
            "SonIlac",
            tarihSaat
        );

        PlayerPrefs.Save();

        if (gecmisText != null)
        {
            gecmisText.text =
                "Son ilaç: " + tarihSaat;
        }

        kacirdi = 0;

        BildirimGoster(
            "Ýlacýnýzý aldýnýz!\nKaraciðer saðlýklý.",
            new Color(0.11f, 0.62f, 0.46f, 0.95f)
        );
    }

    // ============================================================
    // ILAC ATLAMA
    // ============================================================

    public void IlacAtla()
    {
        if (!kararBekleniyor) return;

        kararBekleniyor = false;

        LiverRengi(riskliRenk);

        scoreManager?.PuanCikar(5);

        ButonlariAktiflestir(false);

        if (kararPanel != null)
            kararPanel.SetActive(false);

        kacirdi++;

        if (kacirdi >= 2)
        {
            BildirimGoster(
                "Ýlaç tekrar atlandý!\nOrgan reddi riski artýyor!",
                new Color(0.89f, 0.2f, 0.2f, 0.95f)
            );
        }
        else
        {
            BildirimGoster(
                "Ýlaç alýnmadý!\nKaraciðer risk altýnda.",
                new Color(0.89f, 0.29f, 0.29f, 0.95f)
            );
        }
    }

    // ============================================================

    void SifirlaVeDevam()
    {
        RenkSifirla();

        HerSeyleriGizle();
    }

    // ============================================================
    // BILDIRIM
    // ============================================================

    void BildirimGoster(string mesaj, Color renk)
    {
        if (bildirimPanel == null) return;

        if (bildirimText != null)
        {
            bildirimText.gameObject.SetActive(true);

            bildirimText.text = mesaj;
        }

        Image img =
            bildirimPanel.GetComponent<Image>();

        if (img != null)
            img.color = renk;

        if (aktifFade != null)
            StopCoroutine(aktifFade);

        aktifFade =
            StartCoroutine(FadeGoster());
    }

    // ============================================================

    IEnumerator FadeGoster()
    {
        CanvasGroup cg =
            bildirimPanel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = bildirimPanel.AddComponent<CanvasGroup>();

        bildirimPanel.SetActive(true);

        cg.alpha = 0f;

        float t = 0f;

        while (t < fadeSuresi)
        {
            t += Time.deltaTime;

            cg.alpha =
                Mathf.Clamp01(t / fadeSuresi);

            yield return null;
        }

        cg.alpha = 1f;
    }

    // ============================================================

    void ButonlariAktiflestir(bool durum)
    {
        if (butonA != null)
            butonA.interactable = durum;

        if (butonB != null)
            butonB.interactable = durum;
    }

    // ============================================================

    void LiverRengi(Color renk)
    {
        if (liverObject == null) return;

        Renderer[] renderers =
            liverObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.material.color = renk;
        }
    }

    // ============================================================

    void RenkSifirla()
    {
        LiverRengi(varsayilanRenk);
    }
}