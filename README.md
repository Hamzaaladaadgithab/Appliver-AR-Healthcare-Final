# APPLIVER - Karaciğer Nakli Hastaları İçin Mobil AR Eğitim ve Takip Uygulaması

## Öğrenci Bilgileri

**Ad Soyad:** Hamza ALadaad
**Öğrenci No:** 220541611
**Öğrenci E-posta:** [220541611@firat.edu.tr](mailto:220541611@firat.edu.tr)
**Üniversite:** Fırat Üniversitesi
**Bölüm:** Yazılım Mühendisliği

---

APPLIVER, karaciğer nakli hastaları ve hasta yakınları için geliştirilmiş Unity tabanlı mobil bir eğitim ve takip uygulamasıdır. Proje, hastaların tedavi sürecini daha iyi anlamasına, günlük bakım görevlerini takip etmesine ve artırılmış gerçeklik destekli 3D karaciğer simülasyonu ile eğitim almasına yardımcı olmayı amaçlar.

## Projenin Amacı

Bu projenin amacı, karaciğer nakli sürecindeki hastalara ve hasta yakınlarına mobil ortamda eğitim ve takip desteği sağlamaktır. Uygulama; hasta hikayesi, günlük bakım, tedavi takvimi ve AR karaciğer simülasyonu ile kullanıcıya daha anlaşılır ve etkileşimli bir deneyim sunar.

> Not: Bu uygulama gerçek tıbbi karar vermez. Tedavi takvimindeki veriler demo amaçlıdır. Gerçek kullanımda ilaç saatleri ve tedavi planı doktor reçetesine göre belirlenmelidir.

## Kullanıcılar

* Karaciğer nakli hastaları
* Hasta yakınları
* Bakım veren kişiler
* Sağlık eğitimi alan öğrenciler
* Karaciğer nakli süreci hakkında bilgi almak isteyen kullanıcılar

## Temel Özellikler

* Hasta hikayesi ekranı
* Dashboard / ana kontrol paneli
* Günlük bakım takip ekranı
* Tedavi Takvimi ekranı
* Demo ilaç ve kontrol hatırlatmaları
* Vuforia destekli AR karaciğer simülasyonu
* 3D karaciğer modeli görüntüleme
* AR eğitim senaryosu
* Skor, doğru ve yanlış cevap takibi
* Android cihaz üzerinde çalışma

## Güncel Ekran Yapısı

* Splash Screen
* Patient Story Screen
* Dashboard Screen
* Daily Care Screen
* Treatment Calendar Screen
* AR Simulation Screen
* Doctor History Screen

## Uygulama Akışı

1. Kullanıcı uygulamayı açar.
2. Splash ekranından hasta hikayesi ekranına geçer.
3. Hasta hikayesi ile karaciğer nakli süreci hakkında bilgi alır.
4. Dashboard ekranına geçer.
5. Günlük bakım ekranında görevlerini takip eder.
6. Tedavi Takvimi ekranında demo hatırlatmaları görür.
7. AR Simülasyon ekranına geçer.
8. Kamera ImageTarget algıladığında 3D karaciğer modeli görüntülenir.
9. Kullanıcı AR ortamında eğitim alır.

## Tedavi Takvimi

Tedavi Takvimi ekranı demo veri ile çalışmaktadır. Bu ekranda örnek olarak:

* İlaç hatırlatması
* Su tüketimi kontrolü
* Ateş ölçümü
* Yaklaşan doktor kontrolleri
* Geçmiş doktor kayıtları

gösterilmektedir.

Tedavi Takvimi gerçek veritabanı, Firebase veya gerçek tıbbi doz hesaplama sistemi kullanmaz. Amaç, uygulamanın takip ve hatırlatma mantığını göstermektir.

## APK Dosyası

Uygulamanın Android APK dosyası GitHub Release üzerinden indirilebilir.

[APPLIVER Final APK İndir](https://github.com/Hamzaaladaadgithab/Appliver-AR-Healthcare-Final/releases/tag/v1.0.0)

Uygulama Android cihazda test edilmiştir.

### APK Kurulum Adımları

1. Yukarıdaki APK linkine tıklayın.
2. Release sayfasından APK dosyasını indirin.
3. Android telefonda bilinmeyen kaynaklardan yüklemeye izin verin.
4. APK dosyasını açıp yükleyin.
5. APPLIVER uygulamasını çalıştırın.
6. AR simülasyon için kamerayı ImageTarget üzerine tutun.

> Not: Uygulama demo/eğitim amaçlıdır ve gerçek tıbbi karar vermez.

## Demo Video

Proje demo videosu repoda ana dizinde `Demo_video.mp4` dosyası olarak bulunmaktadır.

Ayrıca demo video YouTube Shorts üzerinden de izlenebilir:

[APPLIVER Demo Video - YouTube Shorts](https://youtube.com/shorts/Sv1axtOP8eg?feature=share)

Videoda uygulamanın Dashboard, Günlük Bakım, Tedavi Takvimi ve AR Simülasyon bölümleri gösterilmektedir.

## Kullanılan Teknolojiler

* Unity 2022.3 LTS
* C#
* Vuforia Engine
* TextMeshPro
* Android Build
* Git & GitHub

## Önemli Scriptler

| Dosya                          | Görevi                                         |
| ------------------------------ | ---------------------------------------------- |
| AppNavigationManager.cs        | Ekranlar arası geçişleri yönetir               |
| TreatmentCalendarController.cs | Tedavi Takvimi ekranını ve butonlarını yönetir |
| DailyCareController.cs         | Günlük bakım görevlerini takip eder            |
| ScenarioManager.cs             | AR eğitim senaryolarını yönetir                |
| ScoreManager.cs                | Skor, doğru ve yanlış değerlerini yönetir      |
| RotateLiver.cs                 | 3D karaciğer modelinin dönmesini sağlar        |

## Kurulum ve Çalıştırma

1. Proje Unity 2022.3 LTS ile açılır.
2. `Assets/Scenes/SampleScene.unity` sahnesi açılır.
3. Build Settings üzerinden platform Android olarak seçilir.
4. Android cihaz USB ile bilgisayara bağlanır.
5. USB Debugging ve Install via USB ayarları aktif edilir.
6. Build And Run ile uygulama telefona yüklenir.
7. AR simülasyon için kamera ImageTarget üzerine tutulur.

## GitHub Repo Yapısı

Ders gereksinimlerine göre repoda aşağıdaki dosyalar bulunmaktadır:

```text
/docs
   SWOT.pdf
   RAMS.pdf
   THS_report.pdf
   Requirements.pdf
   UserScenario.pdf

README.md
Trello_link.txt
Demo_video.mp4
```

## SWOT Özeti

| Başlık       | Açıklama                                                                                 |
| ------------ | ---------------------------------------------------------------------------------------- |
| Güçlü Yönler | AR destekli eğitim, mobil kullanım, sade arayüz, günlük takip                            |
| Zayıf Yönler | Gerçek veritabanı yok, tedavi takvimi demo veridir, gerçek tıbbi karar vermez            |
| Fırsatlar    | Gerçek bildirim sistemi, doktor-hasta iletişimi ve veritabanı eklenebilir                |
| Tehditler    | Yaşlı kullanıcıların zorlanması, cihaz uyumluluğu ve tıbbi bilgilerin yanlış anlaşılması |

## RAMS Özeti

| RAMS            | Projeye Göre Açıklama                                                      |
| --------------- | -------------------------------------------------------------------------- |
| Reliability     | Uygulama ekranları ve AR simülasyon stabil çalışmalıdır                    |
| Availability    | Uygulama Android telefonda kolayca kullanılabilir olmalıdır                |
| Maintainability | Kodlar ayrı scriptler halinde düzenlendiği için geliştirilebilir yapıdadır |
| Safety          | Uygulama gerçek tıbbi karar vermez ve ilaç dozu belirlemez                 |

## THS Değerlendirmesi

Bu proje THS 7’ye yakın çalışan bir sistem prototipi olarak değerlendirilebilir.

**Verilen puan:** 80

Gerekçe:

* Uygulama Android cihazda çalışmaktadır.
* Vuforia ile AR karaciğer simülasyonu yapılmaktadır.
* Temel ekranlar ve modüller çalışmaktadır.
* Canlı demo yapılabilir durumdadır.
* Ancak gerçek hastane sistemi, gerçek veritabanı ve klinik kullanım olmadığı için THS 8 veya THS 9 seviyesinde değildir.

## Sınırlılıklar

* Tedavi Takvimi demo veri kullanır.
* Gerçek tıbbi karar vermez.
* Gerçek veritabanı bağlantısı yoktur.
* Firebase veya harici sağlık sistemi entegrasyonu yoktur.
* Klinik kullanım için doktor onayı, güvenli veri yönetimi ve kapsamlı testler gerekir.

## Demo Sırası

Sunum sırasında önerilen demo akışı:

1. Uygulamayı Android telefonda açma
2. Dashboard ekranı
3. Günlük bakım ekranı
4. Tedavi Takvimi ekranı
5. AR simülasyon ekranı
6. ImageTarget ile 3D karaciğer modelini gösterme
7. Ana ekrana dönüş

## Geliştirici

Hamza ALadaad
Fırat Üniversitesi
Yazılım Mühendisliği
