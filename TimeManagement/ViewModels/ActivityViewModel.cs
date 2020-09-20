﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.ViewModels;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class ActivityViewModel:BaseViewModel
    {
        public string Day => DateTime.Today.DayOfWeek.ToString().ToUpper();
        public string ActualActivityTime => string.Format(_actualActivityStart.Hours + ":" + _actualActivityStart.Minutes + "-" + _actualActivityEnd.Hours + ":" + _actualActivityEnd.Minutes);
        public string NextActivityTime => string.Format(_nextActivityStart.Hours + ":" + _nextActivityStart.Minutes + "-" + _nextActivityEnd.Hours + ":" + _nextActivityEnd.Minutes);
        public string PreviousActivityTime => string.Format(_previousActivityStart.Hours + ":" + _previousActivityStart.Minutes + "-" + _previousActivityEnd.Hours + ":" + _previousActivityEnd.Minutes);
        
        private string _actualActivityName;
        public string ActualActivityName
        {
            set => SetValue(ref _actualActivityName, value);
            get => _actualActivityName;
        }
        
        private string _nextActivityName;
        public string NextActivityName
        {
            set => SetValue(ref _nextActivityName, value);
            get => _nextActivityName;
        }
        
        private string _previousActivityName;
        public string PreviousActivityName
        {
            set => SetValue(ref _previousActivityName, value);
            get => _previousActivityName;
        }

        private TimeSpan _actualActivityStart;
        private TimeSpan _actualActivityEnd;
        private TimeSpan _previousActivityStart;
        private TimeSpan _previousActivityEnd;
        private TimeSpan _nextActivityStart;
        private TimeSpan _nextActivityEnd;

        private readonly Activity _actualActivity;

        private Dowloanding _dowloanding;
        private int _value;
        private List<Activity> _activities;
        private readonly SqLiteService _sqLiteService;

        public ActivityViewModel()
        {
            _sqLiteService = new SqLiteService();
            _activities = new List<Activity>(_sqLiteService.ToListAsync().Result);
            _actualActivity = _activities
                .Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
            _value = 0;
            Default();
        }

        public async Task LoadNewData()
        {
            _dowloanding.Download();
            _activities = new List<Activity>(await _sqLiteService.ToListAsync());
        }

        public void Add()
        {
            _value++;
            NextAndPrevious(_value);
        }
        
        public void Previous()
        {
            _value--;
            NextAndPrevious(_value);
        }
        
        private async void Default()
        {
            Activity previousActivity = _activities[_activities.IndexOf(_actualActivity)-1];// .Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).Where(activity => activity.Start <= DateTime.Now.TimeOfDay).Reverse().Skip(1).Take(1).FirstOrDefault();
            Activity nextActivity = _activities[_activities.IndexOf(_actualActivity)+1]; //*Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).FirstOrDefault(activity => activity.Start >= DateTime.Now.TimeOfDay);
            
            _actualActivityStart = _actualActivity.Start;
            _actualActivityEnd = _actualActivity.End;
            ActualActivityName = _actualActivity.Name;

            _previousActivityStart = previousActivity.Start;
            _previousActivityEnd = previousActivity.End;
            PreviousActivityName = previousActivity.Name;
            
            _nextActivityStart = nextActivity.Start;
            _nextActivityEnd = nextActivity.End;
            NextActivityName = nextActivity.Name;
        }

        private async void NextAndPrevious(int nextItems)
        {
            Activity actualActivity = _activities[_activities.IndexOf(_actualActivity)+nextItems];
            Activity previousActivity = _activities[_activities.IndexOf(actualActivity)-1];
            Activity nextActivity = _activities[_activities.IndexOf(actualActivity)+1];
            
            _actualActivityStart = actualActivity.Start;
            _actualActivityEnd = actualActivity.End;
            ActualActivityName = actualActivity.Name;

            _previousActivityStart = previousActivity.Start;
            _previousActivityEnd = previousActivity.End;
            PreviousActivityName = previousActivity.Name;
            
            _nextActivityStart = nextActivity.Start;
            _nextActivityEnd = nextActivity.End;
            NextActivityName = nextActivity.Name;
        }
    }
}