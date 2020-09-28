using System;
using TimeManagement.ViewModels;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellView : Xamarin.Forms.Shell
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private async void RefreshData_OnClicked(object sender, EventArgs e)
        {
           new ShellViewModel().Refresh.Execute(null);
        }
        private async void Logout_OnClicked(object sender, EventArgs e)
        {
            new ShellViewModel().LogoutCommand.Execute(null);
        }
        private void About_OnClicked(object sender, EventArgs e)
        {
            new ShellViewModel().About.Execute(null);
        }
        
    }
}