using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Helpers;
using FoodOrderApp.Services;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    class LoginViewModel : BaseViewModel
    {        
        private readonly PageService _pageService;
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        
        private string _username;
        public string Username
        {
            set => SetValue(ref _username, value);
            get => _username;
        }

        private string _password;
        public string Password
        {
            set => SetValue(ref _password, value);
            get => _password;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            set => SetValue(ref _isBusy, value);
            get => _isBusy;
        }

        private bool _result;
        public bool Result
        {
            set => SetValue(ref _result, value);
            get => _result;
        }
        
        private bool _disable;
        public bool Disable
        {
            set => SetValue(ref _disable, value);
            get => _disable;
        }

        public LoginViewModel()
        {
            Disable = false;
            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
            _pageService = new PageService();
        }

        private async Task RegisterCommandAsync()
        {
            if (IsBusy)
                return;
            try
            {
                await AnalyticsHelper.TrackEventAsync($"Register Command Executing for {Username}");
                await Register();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await CrashesHelper.TrackErrorAsync(ex, new Dictionary<string, string>
                {
                    {"username", Username}
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Register()
        {
            IsBusy = true;
            var userService = new UserService();
            Result = await userService.RegisterUser(Username, Password);
            if (Result)
                await _pageService.DisplayAlert("Success", "User Registered", "OK");
            else
                await _pageService.DisplayAlert("Error", "User already exists with this credentials", "OK");
        }

        private async Task LoginCommandAsync()
        {
            if (IsBusy)
                return;
            try
            {
                await AnalyticsHelper.TrackEventAsync($"Login Command Executing for {Username}");
                await Login();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await CrashesHelper.TrackErrorAsync(ex, new Dictionary<string, string>
                {
                    {"username", Username}
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Login()
        {
            IsBusy = true;
            var userService = new UserService();
            Result = await userService.Login(Username, Password);
            if (Result)
            {
                _pageService.SetUsername(Username);
                await _pageService.PushModalAsync(new SettingsAndActivityMasterView());
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Invalid Username or Password", "OK");
            }
        }
    }
}
