using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class AboutViewModel:BaseViewModel
    {
        public string Username => _pageService.ReturnUsername().Result;
      
        private PageService _pageService;
        public AboutViewModel()
        {
            _pageService = new PageService();
        }
    }
}