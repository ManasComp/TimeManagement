using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodOrderApp.Services
{
    public class PageService
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public string ReturnUsername(string original = "Guest")
        {
            return Preferences.Get("Username", original);
        }

        public void SetUsername(string value)
        {
            Preferences.Set("Username", value);
        }

        public void RemoveUsername()
        {
            Preferences.Remove("Username");
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

        public bool GetIsCartTableCreated()
        {
           return Preferences.Get("isCartItemTableCreated", false);
        }
        
        public async void SetIsCartTableCreated(bool value)
        {
            Preferences.Set("isCartItemTableCreated", value);
        }
        
    }
}