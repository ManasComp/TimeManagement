using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Helpers;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class LogoutViewModel:BaseViewModel
    {
        private readonly PageService _pageService;
        public ICommand LogoutCommand { get; set; }
        
        public ICommand GoToActivityViewCommand { get; set; }

        public LogoutViewModel()
        {
            LogoutCommand = new Command(async() => await LogoutUserAsync());
            GoToActivityViewCommand = new Command(async() => await GoToActivityView());
            _pageService = new PageService();
        }

        private async Task GoToActivityView()
        {
            await AnalyticsHelper.TrackEventAsync($"GoToCartAsync");
            await _pageService.PushModalAsync(new SettingsAndActivityMasterView());
        }

        private SqLiteService sqLiteService;
        private async Task LogoutUserAsync()
        {
            bool wantLogout = await _pageService.DisplayAlert("Warning", "Do you really want to log out?", "Yes", "No");
            if (wantLogout)
            {
                await AnalyticsHelper.TrackEventAsync($"LogoutUser");
                _pageService.RemoveUsername();
                sqLiteService = new SqLiteService();
                await sqLiteService.DeleteAllAsync();
                await _pageService.PushModalAsync(new LoginView());
            }
        }
    }
}