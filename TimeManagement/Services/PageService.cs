using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class PageService
    {
        private const string username = "Username";
        private const string id = "Id";
        private const string guest = "Guest";
        private const string guestId = "GuestId";
        private const string isCartItemTableCreated = "isCartItemTableCreated";
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> ReturnUsername(string defaultValue= guest)
        {
            return Preferences.Get(username, defaultValue);
        }

        public Task SetUsername(string value)
        {
            Preferences.Set(username, value);
            return Task.CompletedTask;
        }
        
        public async Task<string> ReturnId(string defaultValue = guestId)
        {
            return Preferences.Get(id, defaultValue);
        }

        public Task SetId(string value)
        {
            Preferences.Set(id, value);
            return Task.CompletedTask;
        }

        public Task RemoveUsername()
        {
            Preferences.Remove(username);
            return Task.CompletedTask;
        }

        public async Task PushModalAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }
        
        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task PopModalAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task<bool> GetIsCartTableCreated()
        {
           return Preferences.Get(isCartItemTableCreated, false);
        }
        
        public async Task SetIsCartTableCreated(bool value)
        {
            Preferences.Set(isCartItemTableCreated, value);
        }
        
        public async Task RestartApp()
        {
            (Application.Current).MainPage = new ShellView();
        }

        public async Task Vibrate(int duration = 250)
        {
            Vibration.Vibrate(duration);
        }

        public async void MessagingCenterSend<T>(T sender, string massage) where T : class
        {
            MessagingCenter.Send<T>(sender, massage);
        }
        
        public async void MessagingCenterSubscribe<T>(T sender, string massage, ICommand command, object parametr = null) where T : class
        {
            MessagingCenter.Subscribe<T>(sender, massage, (senderClass) =>
            {
                command.Execute(parametr);
            });
        }

    }
}