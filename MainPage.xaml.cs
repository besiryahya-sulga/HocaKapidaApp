using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;
using System.Collections.ObjectModel; // Liste yönetimi için gerekli

namespace HocaKapidaApp;

public class OgrenciBildirim
{
    public string mesaj { get; set; } // HTML'deki 'mesaj' ile aynı
    public string tur { get; set; }   // HTML'deki 'tur' ile aynı
    public string tarih { get; set; } // HTML'deki 'tarih' ile aynı
    public string full_date { get; set; }
}

public partial class MainPage : ContentPage
{
    FirebaseClient firebaseBaglantisi = new FirebaseClient("https://hocalar-ac5f7-default-rtdb.europe-west1.firebasedatabase.app/");

    public ObservableCollection<OgrenciBildirim> GelenBildirimler { get; set; } = new ObservableCollection<OgrenciBildirim>();

    public MainPage()
    {
        InitializeComponent();

        BildirimlerListesi.ItemsSource = GelenBildirimler;

        firebaseBaglantisi
            .Child("OgrenciBildirimleri")
            .Child("hoca_1")
            .AsObservable<OgrenciBildirim>()
            .Subscribe(yeniBildirim =>
            {
                if (yeniBildirim != null && yeniBildirim.Object != null && !string.IsNullOrEmpty(yeniBildirim.Object.mesaj))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        GelenBildirimler.Insert(0, yeniBildirim.Object);

                        if (GelenBildirimler.Count > 50)
                        {
                            GelenBildirimler.RemoveAt(GelenBildirimler.Count - 1);
                        }
                    });
                }
            });
    }

    private async void OdadaButonunaBasildi(object sender, EventArgs e) => await VeritabaninaYaz("Odada", true);
    private async void DersteButonunaBasildi(object sender, EventArgs e) => await VeritabaninaYaz("Derste", false);
    private async void DisaridaButonunaBasildi(object sender, EventArgs e) => await VeritabaninaYaz("Kampüs Dışında", false);
    private async void RahatsizEtmeyinButonunaBasildi(object sender, EventArgs e) => await VeritabaninaYaz("Rahatsız Etmeyin", false);
    private async void BildirimSilClicked(object sender, EventArgs e)
    {
        var buton = sender as Button;
        var silinecekBildirim = buton.CommandParameter as OgrenciBildirim;

        if (silinecekBildirim != null)
        {
            bool onay = await DisplayAlert("⚠️ Bildirimi Sil",
                "Bu bildirim Firebase üzerinden de KALICI olarak silinecektir. Emin misiniz?",
                "Evet, Sil", "Hayır, İptal");

            if (onay)
            {
                try
                {
                    var tumBildirimler = await firebaseBaglantisi
                        .Child("OgrenciBildirimleri")
                        .Child("hoca_1")
                        .OnceAsync<OgrenciBildirim>();

                    foreach (var kayit in tumBildirimler)
                    {
                        if (kayit.Object.mesaj == silinecekBildirim.mesaj &&
                            kayit.Object.tarih == silinecekBildirim.tarih)
                        {
                            await firebaseBaglantisi
                                .Child("OgrenciBildirimleri")
                                .Child("hoca_1")
                                .Child(kayit.Key)
                                .DeleteAsync();
                        }
                    }

                    GelenBildirimler.Remove(silinecekBildirim);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hata!", "Firebase bağlantısında bir sorun oluştu: " + ex.Message, "Tamam");
                }
            }
        }
    }

    private async Task VeritabaninaYaz(string yeniDurum, bool musaitMi)
    {
        string hocaninNotu = MesajKutusu.Text ?? "";

        var gidenVeri = new HocaBilgisi
        {
            isim = "KÜRŞAD UÇAR",
            durum = yeniDurum,
            isAvailable = musaitMi,
            ozelMesaj = hocaninNotu
        };

        await firebaseBaglantisi
            .Child("Hocalar")
            .Child("hoca_1")
            .PutAsync(gidenVeri);

        HocaIsimLabel.Text = "Hoca: " + gidenVeri.isim;
        DurumLabel.Text = "Durum: " + gidenVeri.durum;

        if (string.IsNullOrWhiteSpace(hocaninNotu))
        {
            OzelMesajLabel.Text = "Not: Yok";
        }
        else
        {
            OzelMesajLabel.Text = "Not: " + hocaninNotu;
        }

        DurumLabel.TextColor = gidenVeri.isAvailable ? Colors.Green : Colors.Red;
        MesajKutusu.Text = "";
    }
}

public class HocaBilgisi
{
    public string isim { get; set; } = "";
    public string durum { get; set; } = "";
    public bool isAvailable { get; set; }
    public string ozelMesaj { get; set; } = "";
}