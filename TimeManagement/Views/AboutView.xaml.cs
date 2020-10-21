using System;
using TimeManagement.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutView : ContentPage
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await new PageService().PopModalAsync();
        }
    }
}