using System;
using System.Net.Mime;
using System.Threading.Tasks;
using TimeManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellView : Xamarin.Forms.Shell
    {
        private readonly ShellViewModel _shellViewModel;
        public ShellView()
        {
            InitializeComponent();
            _shellViewModel = new ShellViewModel();
        }

        private async void RefreshData_OnClicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = false;
            _shellViewModel.Refresh.Execute(null);
        }
        private async void Logout_OnClicked(object sender, EventArgs e)
        { 
            _shellViewModel.LogoutCommand.Execute(null);
        }
        private void About_OnClicked(object sender, EventArgs e)
        {
            _shellViewModel.About.Execute(null);
        }
        
    }
}