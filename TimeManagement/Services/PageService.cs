using System.Threading.Tasks;
using TimeManagement.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class PageService
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> ReturnUsername(string defaultValue= "Guest")
        {
            return Preferences.Get("Username", defaultValue);
        }

        public Task SetUsername(string value)
        {
            Preferences.Set("Username", value);
            return Task.CompletedTask;
        }
        
        public async Task<string> ReturnId(string defaultValue = "GuestId")
        {
            return Preferences.Get("Id", defaultValue);
        }

        public Task SetId(string value)
        {
            Preferences.Set("Id", value);
            return Task.CompletedTask;
        }

        public Task RemoveUsername()
        {
            Preferences.Remove("Username");
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
           return Preferences.Get("isCartItemTableCreated", false);
        }
        
        public async Task SetIsCartTableCreated(bool value)
        {
            Preferences.Set("isCartItemTableCreated", value);
        }
        
        public async Task RestartApp()
        {
            (Application.Current).MainPage = new ShellView();
        }
    }
}