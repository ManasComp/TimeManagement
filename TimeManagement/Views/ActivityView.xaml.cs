using FoodOrderApp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityView : ContentPage
    {
        public ActivityView()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnalyticsHelper.TrackEventAsync("ActivityView opened");
        }
    }
}