﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrderApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutView : ContentPage
    {
        public LogoutView()
        {
            InitializeComponent();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnalyticsHelper.TrackEventAsync("LogoutView opened");
        }
    }
}