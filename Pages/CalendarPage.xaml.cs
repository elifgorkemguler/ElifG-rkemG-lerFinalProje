using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Proje.Services;
using Proje.Pages;
using Proje.Models;
using System.Collections.Generic;

namespace Proje.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private DateTime CurrentMonth;
        private DateTime[] PastPeriods;
        private int AverageCycleDays;
        private List<DateTime> PredictedPeriods;

        public CalendarPage(DateTime[] pastPeriods)
        {
            InitializeComponent();

            // PastPeriods null kontrol�
            if (pastPeriods == null || pastPeriods.Length == 0)
            {
                PastPeriods = new[] { DateTime.Now };
            }
            else
            {
                PastPeriods = pastPeriods.OrderBy(d => d).ToArray();
            }

            // Ortalama d�ng� hesaplama
            AverageCycleDays = CalculateAverageCycleDays(PastPeriods);
            CurrentMonth = DateTime.Now;

            // PredictedPeriods'u initialize et
            PredictedPeriods = new List<DateTime>();
            GeneratePredictedPeriods();

            UpdateCalendar();
        }

        private void GeneratePredictedPeriods()
        {
            PredictedPeriods.Clear(); // �nceki tahminleri temizle

            var lastPeriod = PastPeriods.Max();

            // Ge�mi� periodlar� ekle
            PredictedPeriods.AddRange(PastPeriods);

            for (int i = 0; i < 12; i++)
            {
                var nextPeriod = lastPeriod.AddDays(AverageCycleDays * (i + 1));
                PredictedPeriods.Add(nextPeriod);
            }
            // Tarihe g�re s�rala
            PredictedPeriods = PredictedPeriods.OrderBy(d => d).ToList();

        }

        private int CalculateAveragePeriodDuration(DateTime[] periods)
        {
            if (periods == null || periods.Length < 3)
                return 5; // Varsay�lan s�re

            List<int> durations = new List<int>();
            for (int i = 0; i < periods.Length - 1; i++)
            {
                var duration = (periods[i + 1] - periods[i]).Days;
                if (duration > 0 && duration <= 10) // Ge�erli regl s�resi kontrol�
                {
                    durations.Add(duration);
                }
            }

            return durations.Count > 0 ? (int)Math.Round(durations.Average()) : 5;
        }

        private void GenerateCalendarDays(DateTime targetMonth)
        {
            CalendarDaysGrid.Children.Clear();
            var startOfMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            int averagePeriodDuration = CalculateAveragePeriodDuration(PastPeriods);

            int startDayOfWeek = (int)startOfMonth.DayOfWeek;
            if (startDayOfWeek == 0) startDayOfWeek = 7;

            CalendarDaysGrid.RowDefinitions.Clear();
            int totalWeeks = (int)Math.Ceiling((endOfMonth.Day + startDayOfWeek - 1) / 7.0);

            for (int i = 0; i < totalWeeks; i++)
            {
                CalendarDaysGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            int row = 0;
            int col = startDayOfWeek - 1;

            for (var date = startOfMonth; date <= endOfMonth; date = date.AddDays(1))
            {
                var (isPeriod, isOvulationDay, isFertileDay, isPredictedPeriod) = CalculateDayStatus(date, averagePeriodDuration);

                Color backgroundColor;
                if (isPeriod)
                    backgroundColor = Colors.Red;          // Regl g�nleri - K�rm�z�
                else if (isPredictedPeriod)
                    backgroundColor = Colors.Pink;         // Gelecek regl g�nleri - Pembe
                else if (isOvulationDay)
                    backgroundColor = Colors.LightGreen;   // Yumurtlama g�nleri - A��k ye�il
                else if (isFertileDay)
                    backgroundColor = Colors.LightBlue;    // Verimli g�nler - A��k mavi
                else
                    backgroundColor = Colors.Transparent;

                var frame = CreateDayFrame(date.Day.ToString(), backgroundColor, isPeriod || isPredictedPeriod);
                CalendarDaysGrid.Children.Add(frame);
                Grid.SetColumn(frame, col);
                Grid.SetRow(frame, row);

                col++;
                if (col == 7)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private (bool isPeriod, bool isOvulationDay, bool isFertileDay, bool isPredictedPeriod) CalculateDayStatus(DateTime date, int periodDuration)
        {
            bool isPeriod = false;
            bool isOvulationDay = false;
            bool isFertileDay = false;
            bool isPredictedPeriod = false;

            foreach (var periodStart in PredictedPeriods)
            {
                // Ge�mi� ve gelecek regl g�nlerini ay�rt et
                if (date >= periodStart && date < periodStart.AddDays(periodDuration))
                {
                    if (periodStart > DateTime.Now)
                    {
                        isPredictedPeriod = true;
                    }
                    else
                    {
                        isPeriod = true;
                    }
                    break;
                }

                // Yumurtlama g�n� (d�ng�n�n ortas�)
                var ovulationDate = periodStart.AddDays(AverageCycleDays / 2);
                if (date.Date == ovulationDate.Date)
                {
                    isOvulationDay = true;
                }

                // Verimli g�nler (yumurtlama �ncesi 5 g�n ve sonras� 1 g�n)
                if (date >= ovulationDate.AddDays(-5) && date <= ovulationDate.AddDays(1))
                {
                    isFertileDay = true;
                }
            }

            return (isPeriod, isOvulationDay, isFertileDay, isPredictedPeriod);
        }


        private Frame CreateDayFrame(string dayText, Color backgroundColor, bool isPeriod)
        {
            return new Frame
            {
                BackgroundColor = backgroundColor,
                CornerRadius = 5,
                Padding = 5,
                Margin = 2,
                Content = new Label
                {
                    Text = dayText,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = isPeriod ? Colors.White : Colors.Black
                }
            };
        }

        private int CalculateAverageCycleDays(DateTime[] periods)
        {
            if (periods == null || periods.Length < 2)
                return 28; // Varsay�lan d�ng� s�resi

            var cycles = new List<int>();
            for (int i = 0; i < periods.Length - 1; i++)
            {
                var cycle = (periods[i + 1] - periods[i]).Days;
                if (cycle >= 21 && cycle <= 35) // Normal d�ng� aral���
                {
                    cycles.Add(cycle);
                }
            }

            return cycles.Count > 0 ? (int)Math.Round(cycles.Average()) : 28;
        }

        private void UpdateCalendar()
        {
            MonthLabel.Text = CurrentMonth.ToString("MMMM yyyy");
            GenerateCalendarDays(CurrentMonth);
        }

        private void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            UpdateCalendar();
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            UpdateCalendar();
        }

        private async void OnPregnancyTrackerClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PregnancyTrackerPage());
        }

        private async void OnExercisesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExercisesPage());
        }
    }
}