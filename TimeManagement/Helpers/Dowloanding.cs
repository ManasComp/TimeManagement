using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Models;
using TimeManagement.Services;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.Helpers
{
    public class Dowloanding 
    {
        private List<DayProgram> _activities;
        private readonly FirebaseService _firebaseService;
        private readonly SqLiteService _sqLiteService;
        private readonly PageService _pageService;

        public Dowloanding()
        {
            _firebaseService = new FirebaseService();
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
        }

        public async Task Download()
        {
            _activities = (await _firebaseService.OnceAsync<List<DayProgram>>(_pageService.ReturnId().Result)).FirstOrDefault();
            await _sqLiteService.DeleteAllAsync();
            if (_activities != null)
            {
                foreach (DayProgram program in _activities)
                {
                    foreach (Activity activity in program)
                    {
                        activity.Day = _activities.IndexOf(program);
                        await _sqLiteService.InsertAsync(activity);
                    }
                }
                _pageService.RestartApp();
            }
        }
    }
}