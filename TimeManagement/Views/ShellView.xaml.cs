using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrderApp.Services;
using TimeManagement.ViewModels;
using Xamarin.Forms;
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
           await  new SettingsViewModel().LoadNewData();
        }
        private void Logout_OnClicked(object sender, EventArgs e)
        {
            new PageService().PushModalAsync(new LogoutView());
        }
        private void About_OnClicked(object sender, EventArgs e)
        {
            new PageService().PushModalAsync(new NavigationPage(new AboutView()));
        }
        
    }
}