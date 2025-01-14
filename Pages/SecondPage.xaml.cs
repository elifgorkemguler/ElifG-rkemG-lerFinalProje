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
            // Regl 1 S�resi Hesaplama
            if (Regl1Start.Date <= Regl1End.Date)
            {
                int duration = (Regl1End.Date - Regl1Start.Date).Days + 1;
                Regl1Duration.Text = $"1. Regl S�resi: {duration} g�n";
            }
            else
            {
                Regl1Duration.Text = "1. Regl S�resi: Ge�ersiz Tarih!";
            }

            // Regl 2 S�resi Hesaplama
            if (Regl2Start.Date <= Regl2End.Date)
            {
                int duration = (Regl2End.Date - Regl2Start.Date).Days + 1;
                Regl2Duration.Text = $"2. Regl S�resi: {duration} g�n";
            }
            else
            {
                Regl2Duration.Text = "2. Regl S�resi: Ge�ersiz Tarih!";
            }

            // Regl 3 S�resi Hesaplama
            if (Regl3Start.Date <= Regl3End.Date)
            {
                int duration = (Regl3End.Date - Regl3Start.Date).Days + 1;
                Regl3Duration.Text = $"3. Regl S�resi: {duration} g�n";
            }
            else
            {
                Regl3Duration.Text = "3. Regl S�resi: Ge�ersiz Tarih!";
            }
        }

        private async void OnSaveAndContinueClicked(object sender, EventArgs e)
        {
            // Kullan�c�dan al�nan bilgileri kontrol et
            if (string.IsNullOrWhiteSpace(HeightEntry.Text) || string.IsNullOrWhiteSpace(WeightEntry.Text))
            {
                await DisplayAlert("Hata", "L�tfen boy ve kilo de�erlerini doldurun.", "Tamam");
                return;
            }

            // Boy ve kilo giri�lerini do�rula
            if (!int.TryParse(HeightEntry.Text, out var height) || height <= 0)
            {
                await DisplayAlert("Hata", "Ge�erli bir boy de�eri girin.", "Tamam");
                return;
            }

            if (!int.TryParse(WeightEntry.Text, out var weight) || weight <= 0)
            {
                await DisplayAlert("Hata", "Ge�erli bir kilo de�eri girin.", "Tamam");
                return;
            }

            // Regl ba�lang�� tarihlerini kontrol et
            DateTime[] pastPeriods = new[]
            {
        Regl1Start.Date,
        Regl2Start.Date,
        Regl3Start.Date
    };

            // Kaydedildi mesaj� g�ster
            await DisplayAlert("Ba�ar�l�", "Bilgiler kaydedildi!", "Tamam");

            // CalendarPage'e y�nlendirme
            try
            {
                await Navigation.PushAsync(new CalendarPage(pastPeriods));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sayfaya ge�i�te bir sorun olu�tu: {ex.Message}", "Tamam");
            }
        }

    }
}
