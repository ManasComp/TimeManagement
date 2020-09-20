using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Helpers;
using FoodOrderApp.Services;
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
            await _pageService.PushModalAsync(new ActivityView());
        }

        private async Task LogoutUserAsync()
        {
            await AnalyticsHelper.TrackEventAsync($"LogoutUser");
            _pageService.RemoveUsername();
            await _pageService.PushModalAsync(new LoginView());
        }
    }
}