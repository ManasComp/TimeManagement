using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Helpers;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ShellViewModel:BaseViewModel
    {
        private readonly PageService _pageService;
        public ICommand LogoutCommand { get; set; }

        public ShellViewModel()
        {
            _pageService = new PageService();
            LogoutCommand = new Command(async() => await LogoutUserAsync());
        }

        private SqLiteService _sqLiteService;
        private async Task LogoutUserAsync()
        {
            bool wantLogout = await _pageService.DisplayAlert("Warning", "Do you really want to log out?", "Yes", "No");
            if (wantLogout)
            {
                await AnalyticsHelper.TrackEventAsync($"LogoutUser");
                _pageService.RemoveUsername();
                _sqLiteService = new SqLiteService();
                await _sqLiteService.DeleteAllAsync();
                await _pageService.PushModalAsync(new LoginView());
            }
        }
    }
}