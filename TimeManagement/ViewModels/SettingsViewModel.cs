using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class SettingsViewModel:BaseViewModel
    {
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            set => SetValue(ref _isRefreshing, value);
            get => _isRefreshing;
        }

        public string Username => _pageService.ReturnUsername();
        public ICommand Logout { get; set; }
        public ICommand Refresh { get; set; }
        private PageService _pageService;
        private Dowloanding _dowloanding;
        private readonly SqLiteService _sqLiteService;
        public SettingsViewModel()
        {
            _pageService = new PageService();
            _dowloanding = new Dowloanding();
            Logout = new Command(async () => await _pageService.PushModalAsync(new LogoutView()));
            Refresh = new Command(async () => await LoadNewData());
        }
        public async Task LoadNewData()
        {
            _dowloanding.Download();
            await _pageService.DisplayAlert("Refreshing", "Refreshing completed", "ok");
            //await _pageService.PushAsync(new ShellView());
        }
    }
}