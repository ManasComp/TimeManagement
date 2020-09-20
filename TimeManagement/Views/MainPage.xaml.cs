using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Services;
using Xamarin.Forms;

namespace TimeManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            new SqLiteService().CreateTableAsync();
        }
    }
}
