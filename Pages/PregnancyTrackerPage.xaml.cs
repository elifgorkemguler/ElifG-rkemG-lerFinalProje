using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Proje.Services;
using Proje.Pages;
using Proje.Models;
using System.Collections.Generic;
using System.Timers;


namespace Proje.Pages
{
    public partial class PregnancyTrackerPage : ContentPage
    {
        private const int TotalPregnancyWeeks = 40; // Toplam hamilelik süresi (40 hafta)
        private System.Timers.Timer countdownTimer; // Zamanlayýcý

        public PregnancyTrackerPage()
        {
            InitializeComponent();
        }

        private void OnStartPregnancyButtonClicked(object sender, EventArgs e)
        {
            // Kullanýcýnýn seçtiði tarihi al
            var selectedDate = PregnancyStartDatePicker.Date;

            // Tahmini doðum tarihini hesapla ve göster
            var estimatedDueDate = selectedDate.AddDays(TotalPregnancyWeeks * 7);
            EstimatedDueDateLabel.Text = $"Tahmini Doðum Tarihi: {estimatedDueDate:dd MMMM yyyy}";

            // Hamilelik baþlangýç tarihine göre geri sayýmý baþlat
            StartCountdown(selectedDate);
        }

        private void StartCountdown(DateTime pregnancyStartDate)
        {
            // Zamanlayýcý oluþtur (1 saniyede bir günceller)
            countdownTimer = new System.Timers.Timer(1000);
            countdownTimer.Elapsed += (s, e) => UpdateCountdown(pregnancyStartDate);
            countdownTimer.Start();

            // Geri sayým bilgilerini göster
            CountdownInfoStack.IsVisible = true;
        }

        private void UpdateCountdown(DateTime pregnancyStartDate)
        {
            // Doðum tarihi (40 hafta sonrasý)
            var dueDate = pregnancyStartDate.AddDays(TotalPregnancyWeeks * 7);

            // Kalan süreyi hesapla
            var timeRemaining = dueDate - DateTime.Now;

            // Eðer doðum tarihi geçmiþse zamanlayýcýyý durdur ve mesaj göster
            if (timeRemaining.TotalSeconds <= 0)
            {
                countdownTimer.Stop();

                // Main Thread'de UI güncellemesi için
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DaysRemainingLabel.Text = "Doðum zamaný geldi!";
                    HoursRemainingLabel.Text = string.Empty;
                    MinutesRemainingLabel.Text = string.Empty;
                });
                return;
            }

            // Gün, saat ve dakika bilgilerini hesapla
            int days = timeRemaining.Days;
            int hours = timeRemaining.Hours;
            int minutes = timeRemaining.Minutes;

            // UI güncellemesini Main Thread'de yap
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DaysRemainingLabel.Text = $"Gün: {days}";
                HoursRemainingLabel.Text = $"Saat: {hours}";
                MinutesRemainingLabel.Text = $"Dakika: {minutes}";
            });
        }
    }
}
