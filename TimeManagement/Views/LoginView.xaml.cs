using TimeManagement.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private readonly AnalyticsHelper  _analyticsHelper;
        public LoginView()
        {
            InitializeComponent();
            _analyticsHelper = new AnalyticsHelper();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _analyticsHelper.TrackEventAsync("LoginView opened");
        }
    }
}