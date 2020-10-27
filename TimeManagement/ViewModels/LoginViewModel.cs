using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.Services;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    class LoginViewModel : BaseViewModel
    {        
        private readonly PageService _pageService;
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        private readonly CrashesHelper _crashesHelper;
        private readonly AnalyticsHelper _analyticsHelper;
        
        private string _username;
        public string Username
        {
            set => SetValue(ref _username, value.Trim());
            get => _username;
        }

        private string _password;
        public string Password
        {
            set => SetValue(ref _password, value.Trim());
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

        private readonly UserService _userService;
        private readonly FirebaseService _firebase;
        public LoginViewModel()
        {
            Disable = false;
            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
            _pageService = new PageService();
            _userService = new UserService();
            _firebase = new FirebaseService();
            _crashesHelper = new CrashesHelper();
            _analyticsHelper = new AnalyticsHelper();
            IsBusy = false;
        }

        private async Task RegisterCommandAsync()
        {
            if (IsBusy)
                return;
            if (_pageService.IsNetwork().Result == false)
            {
                await _pageService.DisplayNoInternetAlert();;
                return;
            }
            try
            {
                IsBusy = true;
                await _analyticsHelper.TrackEventAsync($"Register Command Executing for {Username}");
                Register();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await _crashesHelper.TrackErrorAsync(ex, new Dictionary<string, string>
                {
                    {"username", Username}
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void Register()
        {
            Result = await _userService.RegisterUser(Username, Password);
            if (Result)
            {
                await _pageService.DisplayAlert("Success", "You are registered! Upload data and then click login.", "OK");
            }
            else
                await _pageService.DisplayAlert("Error", "User already exists with this credentials", "OK");
        }

        private async Task LoginCommandAsync()
        {
            if (IsBusy)
                return;
            if (_pageService.IsNetwork().Result == false)
            {
                await _pageService.DisplayNoInternetAlert();
                return;
            }
            try
            {
                IsBusy = true;
                await _analyticsHelper.TrackEventAsync($"Login Command Executing for {Username}");
                Login();
                //await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await _crashesHelper.TrackErrorAsync(ex, new Dictionary<string, string>
                {
                    {"username", Username}
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void Login()
        {
            Result = await _userService.Login(Username, Password);
            if (Result)
            {
                await _pageService.SetUsername(Username);
                string id = (await _firebase.OnceAsync<User>("Users"))
                   .Where(u => u.Username == Username)
                   .FirstOrDefault(u => u.Password == Password).Id;
                await _pageService.SetId(id);
                await _pageService.PushModalAsync(new ShellView());
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Invalid Username or Password", "OK");
            }
        }
    }
}
