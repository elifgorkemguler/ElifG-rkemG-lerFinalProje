using Microsoft.Maui.Controls;
using System;
using Proje.Services;
using Proje.Pages;
using Proje.Models;

namespace Proje.Pages
{
    public partial class SecondPage : ContentPage
    {
        public SecondPage()
        {
            InitializeComponent();

            Regl1Start.DateSelected += OnDateSelected;
            Regl1End.DateSelected += OnDateSelected;
            Regl2Start.DateSelected += OnDateSelected;
            Regl2End.DateSelected += OnDateSelected;
            Regl3Start.DateSelected += OnDateSelected;
            Regl3End.DateSelected += OnDateSelected;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Regl 1 Süresi Hesaplama
            if (Regl1Start.Date <= Regl1End.Date)
            {
                int duration = (Regl1End.Date - Regl1Start.Date).Days + 1;
                Regl1Duration.Text = $"1. Regl Süresi: {duration} gün";
            }
            else
            {
                Regl1Duration.Text = "1. Regl Süresi: Geçersiz Tarih!";
            }

            // Regl 2 Süresi Hesaplama
            if (Regl2Start.Date <= Regl2End.Date)
            {
                int duration = (Regl2End.Date - Regl2Start.Date).Days + 1;
                Regl2Duration.Text = $"2. Regl Süresi: {duration} gün";
            }
            else
            {
                Regl2Duration.Text = "2. Regl Süresi: Geçersiz Tarih!";
            }

            // Regl 3 Süresi Hesaplama
            if (Regl3Start.Date <= Regl3End.Date)
            {
                int duration = (Regl3End.Date - Regl3Start.Date).Days + 1;
                Regl3Duration.Text = $"3. Regl Süresi: {duration} gün";
            }
            else
            {
                Regl3Duration.Text = "3. Regl Süresi: Geçersiz Tarih!";
            }
        }

        private async void OnSaveAndContinueClicked(object sender, EventArgs e)
        {
            // Kullanýcýdan alýnan bilgileri kontrol et
            if (string.IsNullOrWhiteSpace(HeightEntry.Text) || string.IsNullOrWhiteSpace(WeightEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen boy ve kilo deðerlerini doldurun.", "Tamam");
                return;
            }

            // Boy ve kilo giriþlerini doðrula
            if (!int.TryParse(HeightEntry.Text, out var height) || height <= 0)
            {
                await DisplayAlert("Hata", "Geçerli bir boy deðeri girin.", "Tamam");
                return;
            }

            if (!int.TryParse(WeightEntry.Text, out var weight) || weight <= 0)
            {
                await DisplayAlert("Hata", "Geçerli bir kilo deðeri girin.", "Tamam");
                return;
            }

            // Regl baþlangýç tarihlerini kontrol et
            DateTime[] pastPeriods = new[]
            {
        Regl1Start.Date,
        Regl2Start.Date,
        Regl3Start.Date
    };

            // Kaydedildi mesajý göster
            await DisplayAlert("Baþarýlý", "Bilgiler kaydedildi!", "Tamam");

            // CalendarPage'e yönlendirme
            try
            {
                await Navigation.PushAsync(new CalendarPage(pastPeriods));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sayfaya geçiþte bir sorun oluþtu: {ex.Message}", "Tamam");
            }
        }

    }
}
