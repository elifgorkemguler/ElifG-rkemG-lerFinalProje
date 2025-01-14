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
        private const int TotalPregnancyWeeks = 40; // Toplam hamilelik s�resi (40 hafta)
        private System.Timers.Timer countdownTimer; // Zamanlay�c�

        public PregnancyTrackerPage()
        {
            InitializeComponent();
        }

        private void OnStartPregnancyButtonClicked(object sender, EventArgs e)
        {
            // Kullan�c�n�n se�ti�i tarihi al
            var selectedDate = PregnancyStartDatePicker.Date;

            // Tahmini do�um tarihini hesapla ve g�ster
            var estimatedDueDate = selectedDate.AddDays(TotalPregnancyWeeks * 7);
            EstimatedDueDateLabel.Text = $"Tahmini Do�um Tarihi: {estimatedDueDate:dd MMMM yyyy}";

            // Hamilelik ba�lang�� tarihine g�re geri say�m� ba�lat
            StartCountdown(selectedDate);
        }

        private void StartCountdown(DateTime pregnancyStartDate)
        {
            // Zamanlay�c� olu�tur (1 saniyede bir g�nceller)
            countdownTimer = new System.Timers.Timer(1000);
            countdownTimer.Elapsed += (s, e) => UpdateCountdown(pregnancyStartDate);
            countdownTimer.Start();

            // Geri say�m bilgilerini g�ster
            CountdownInfoStack.IsVisible = true;
        }

        private void UpdateCountdown(DateTime pregnancyStartDate)
        {
            // Do�um tarihi (40 hafta sonras�)
            var dueDate = pregnancyStartDate.AddDays(TotalPregnancyWeeks * 7);

            // Kalan s�reyi hesapla
            var timeRemaining = dueDate - DateTime.Now;

            // E�er do�um tarihi ge�mi�se zamanlay�c�y� durdur ve mesaj g�ster
            if (timeRemaining.TotalSeconds <= 0)
            {
                countdownTimer.Stop();

                // Main Thread'de UI g�ncellemesi i�in
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DaysRemainingLabel.Text = "Do�um zaman� geldi!";
                    HoursRemainingLabel.Text = string.Empty;
                    MinutesRemainingLabel.Text = string.Empty;
                });
                return;
            }

            // G�n, saat ve dakika bilgilerini hesapla
            int days = timeRemaining.Days;
            int hours = timeRemaining.Hours;
            int minutes = timeRemaining.Minutes;

            // UI g�ncellemesini Main Thread'de yap
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DaysRemainingLabel.Text = $"G�n: {days}";
                HoursRemainingLabel.Text = $"Saat: {hours}";
                MinutesRemainingLabel.Text = $"Dakika: {minutes}";
            });
        }
    }
}
