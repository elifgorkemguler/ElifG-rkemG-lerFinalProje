using Microsoft.Maui.Controls;
using System;
using Proje.Services;
using Proje.Models;
using Proje.Pages;

namespace Proje
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Task.Run(async () => await InitializeDatabaseAsync());
        }

        private static async Task InitializeDatabaseAsync()
        {
            await DatabaseHelper.InitializeDatabaseAsync();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Alanların doldurulup doldurulmadığını kontrol et
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
               string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
               string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
                return;
            }

            // Yeni kullanıcı oluştur
            var newUser = new User
            {
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Email = EmailEntry.Text
            };

            // Veritabanına kaydet
            var db = DatabaseHelper.GetDatabaseConnection();
            await db.InsertAsync(newUser);

            // Kayıt başarılı mesajı göster
            await DisplayAlert("Başarılı", "Kayıt başarılı!", "Tamam");

            // İkinci sayfaya yönlendir
            await Shell.Current.GoToAsync("//SecondPage");

            // Alanları temizle
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
        }
    }
}
