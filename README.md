# HocaKapıda App - Akademisyen Durum Takip Sistemi

##  Proje Hakkında
Üniversite öğrencilerinin akademisyenlere ulaşma sürecini dijitalleştiren ve optimize eden mobil/web entegre bir platformdur. Öğrenciler, akademisyen odalarının kapısındaki QR kodu okutarak hocanın anlık müsaitlik durumunu (Derste, Toplantıda, Kampüs Dışında vb.) saniyeler içinde görebilir. Bu sistem, kampüs içi zaman kaybını önlemek ve iletişimi hızlandırmak amacıyla tasarlanmıştır.

## Kullanılan Teknolojiler ve Mimari
* **Mobil Geliştirme:** `C# (.NET MAUI)` - Çapraz platform (Cross-platform) mobil uygulama altyapısı.
* **Veritabanı ve Backend:** `Firebase Realtime Database` - Akademisyen durumlarının öğrencilere gecikmesiz, anlık olarak yansıması için.
* **Web Arayüzü & Dağıtım:** `HTML` & `Netlify` - QR kodların yönlendirildiği statik web arayüzleri ve hızlı hosting. *(Not: Web arayüzünün UI/UX prototiplenmesi sürecinde yapay zeka araçlarından destek alınarak geliştirme süreci hızlandırılmıştır.)*

## Temel Özellikler
* **Gerçek Zamanlı (Realtime) Senkronizasyon:** Hoca durumunu değiştirdiği an, QR kodu okutan öğrenci anında yeni durumu görür.
* **Hızlı QR Entegrasyonu:** Karmaşık arama veya menülere gerek kalmadan doğrudan bilgiye erişim.
* **Modüler Yapı:** Arka planda güçlü bir C# mimarisi ve esnek bir NoSQL veritabanı bağlantısı.
