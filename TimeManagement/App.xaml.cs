using System;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
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

            isCartTabbleCreated = _pageService.GetIsCartTableCreated();
            
            MainPage = new ActivityView();
        }

        protected override async void OnStart()
        {
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
