﻿using System;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using TimeManagement.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement
{
    public partial class App : Application
    {       
        private readonly PageService _pageService;
        private readonly SqLiteService _sqLiteService;
        
        private static bool isCartTabbleCreated;
        public App()
        {
            
            _pageService = new PageService();
            _sqLiteService = new SqLiteService();
            InitializeComponent();
            
            Crashes.SetEnabledAsync(true);
            Microsoft.AppCenter.Analytics.Analytics.SetEnabledAsync(true);
            isCartTabbleCreated = _pageService.GetIsCartTableCreated();
            string uname = _pageService.ReturnUsername(string.Empty);
            if (String.IsNullOrEmpty(uname))
            {
                MainPage = new LoginView();
            }
            else
            {
                MainPage = new ShellView();
            }
            //MainPage = new SettingsAndActivityMasterView();
        }

        protected override async void OnStart()
        {
            AppCenter.Start("android=be9ae83a-464e-4bd3-9d16-bba27c49a964;" +
                            "uwp={Your UWP App secret here};" +
                            "ios={01f2bb76-98cd-4784-bd90-b81656772444}",
                typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Crashes));
            if (await Crashes.HasCrashedInLastSessionAsync())
            {
                await  _pageService.DisplayAlert("Crash", "Application has crashed", "OK");
            }
            if (isCartTabbleCreated==false)
            {
                await _sqLiteService.CreateTableAsync();
                _pageService.SetIsCartTableCreated(true);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
