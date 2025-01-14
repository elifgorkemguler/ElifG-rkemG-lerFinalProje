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
    public partial class ExercisesPage : ContentPage
    {
        public ExercisesPage()
        {
            InitializeComponent();
        }
        private async void OnExerciseVideoButtonClicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.youtube.com/watch?v=S4UfZ2TV_uA");
            await Launcher.OpenAsync(uri);
        }
        

        private async void OnMeditationVideoButtonClicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.youtube.com/watch?v=inpok4MKVLM");
            await Launcher.OpenAsync(uri);

         }
    }
}
