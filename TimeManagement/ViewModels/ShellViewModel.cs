using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Helpers;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ShellViewModel:BaseViewModel
    {
        private PageService _pageService;
        private Dowloanding _dowloanding;
        private SqLiteService _sqLiteService;
        public ICommand LogoutCommand { get; set; }
        public ICommand Refresh { get; set; }
        public ICommand About { get; set; }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            set => SetValue(ref _isRefreshing, value);
            get => _isRefreshing;
        }

        public ShellViewModel()
        {
            _pageService = new PageService();
            _dowloanding = new Dowloanding();
            _sqLiteService=new SqLiteService();
            Refresh = new Command(async () => await LoadNewData());
            LogoutCommand = new Command(async() => await LogoutUserAsync());
            About = new Command(async() => await _pageService.PushModalAsync(new NavigationPage(new AboutView())));
        }
        private async Task LoadNewData()
        {
            await _dowloanding.Download();
            await _pageService.DisplayAlert("Refreshing", "Refreshing completed!", "ok");
        }
        
        private async Task LogoutUserAsync()
        {
            bool wantLogout = await _pageService.DisplayAlert("Warning", "Do you really want to log out?", "Yes", "No");
            if (wantLogout)
            {
                await AnalyticsHelper.TrackEventAsync($"LogoutUser");
                await _pageService.RemoveUsername();
                _sqLiteService = new SqLiteService();
                await _sqLiteService.DeleteAllAsync();
                await _pageService.PushModalAsync(new LoginView());
            }
        }
    }
}