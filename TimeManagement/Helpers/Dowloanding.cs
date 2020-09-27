using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Models;
using TimeManagement.Services;

namespace TimeManagement.Helpers
{
    public class Dowloanding 
    {
        private List<DayProgram> _activities;
        private readonly FirebaseService _firebaseService;
        private readonly SqLiteService _service;
        private readonly PageService _pageService;

        public Dowloanding()
        {
            _firebaseService = new FirebaseService();
            _service = new SqLiteService();
            _pageService = new PageService();
        }

        public async void Download()
        {
            _activities = (await _firebaseService.OnceAsync<List<DayProgram>>(_pageService.ReturnId())).FirstOrDefault();
            await _service.DeleteAllAsync();
            foreach (DayProgram program in _activities)
            {
                foreach (Activity activity in program)
                {
                    activity.Day = _activities.IndexOf(program);
                    await _service.InsertAsync(activity);
                }
            }
        }
    }
}