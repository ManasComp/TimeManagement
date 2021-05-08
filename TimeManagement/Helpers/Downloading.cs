using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeManagement.Models;
using TimeManagement.Services;

namespace TimeManagement.Helpers
{
    public class Downloading 
    {
        private List<DayProgram> _activities;
        private readonly FirebaseService _firebaseService;
        private readonly SqLiteService _sqLiteService;
        private readonly PageService _pageService;

        public Downloading()
        {
            _firebaseService = new FirebaseService();
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
        }

        public async Task Download()
        {
            if (_pageService.IsNetwork().Result == false)
            {
                await _pageService.DisplayNoInternetAlert();
                return;
            }

            try
            {
                _activities = (await _firebaseService.OnceAsync<List<DayProgram>>(_pageService.ReturnId().Result))
                    .FirstOrDefault(); //there is problem
            }
            catch(Exception ex)
            {
                await _pageService.DisplayAlert("er", ex.Message, "OK");
            }
            
            await _sqLiteService.DeleteAllAsync();
            if (_activities != null)//problem solution
            {
                foreach (DayProgram program in _activities)
                {
                    foreach (Activity activity in program)
                    {
                        activity.Day = _activities.IndexOf(program);
                        await _sqLiteService.InsertAsync(activity);
                    }
                }
                //await _pageService.RestartApp(); //problem solution but if it is enabled, there is IllegalArgumentException
            }
            else
            {
               await _pageService.DisplayAlert("Error", "No data found", "OK");
            }
        }
    }
}