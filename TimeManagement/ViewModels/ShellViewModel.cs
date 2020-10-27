using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Helpers;
using TimeManagement.Services;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ShellViewModel:BaseViewModel
    {
        private readonly PageService _pageService;
        private readonly Downloading _downloading;
        private readonly CrashesHelper _crashesHelper;
        private readonly AnalyticsHelper _analyticsHelper;
        private readonly MessagingCenterHelper _messagingCenterHelper;
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
            _downloading = new Downloading();
            _sqLiteService=new SqLiteService();
            _crashesHelper = new CrashesHelper();
            _analyticsHelper = new AnalyticsHelper();
            _messagingCenterHelper = new MessagingCenterHelper();
            Refresh = new Command(async () => loadNewData());
            LogoutCommand = new Command(async() => await logoutUserAsync());
            About = new Command(async() => await _pageService.PushModalAsync(new NavigationPage(new AboutView())));
        }
        private async Task loadNewData()
        {
            try
            {
                _pageService.MessagingCenterSend<ShellViewModel>(this, _messagingCenterHelper.Refreshing);
                await _downloading.Download();
                _pageService.MessagingCenterSend<ShellViewModel>(this, _messagingCenterHelper.Refreshing);
                await _pageService.DisplayAlert("Refreshing", "Refreshing completed!", "ok");
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await _crashesHelper.TrackErrorAsync(ex);
            }
        }
        
        private async Task logoutUserAsync()
        {
            bool wantLogout = await _pageService.DisplayAlert("Warning", "Do you really want to log out?", "Yes", "No");
            if (wantLogout)
            {
                await _analyticsHelper.TrackEventAsync($"LogoutUser");
                await _pageService.RemoveUsername();
                _sqLiteService = new SqLiteService();
                await _sqLiteService.DeleteAllAsync();
                await _pageService.PushModalAsync(new LoginView());
            }
        }
    }
}