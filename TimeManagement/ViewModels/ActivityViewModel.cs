using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.ViewModels;
using TimeManagement.Views;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class ActivityViewModel : BaseViewModel
    {
        public ICommand Next { get; set; }
        public ICommand Before { get; set; }
        public ICommand Actual { get; set; }
        private string _day;

        public string Day
        {
            set => SetValue(ref _day, value);
            get => _day;
        }

        private string _actualActivityTime;
        public string ActualActivityTime
        {
            set => SetValue(ref _actualActivityTime, value);
            get => _actualActivityTime;
        }
        
        private string _nextActivityTime;
        public string NextActivityTime
        {
            set => SetValue(ref _nextActivityTime, value);
            get => _nextActivityTime;
        }
        
        private string _previousActivityTime;
        public string PreviousActivityTime
        {
            set => SetValue(ref _previousActivityTime, value);
            get => _previousActivityTime;
        }

        public string ToStringFormat(TimeSpan start, TimeSpan end)
        {
            return string.Format($"{start:hh\\:mm}" + " - " + $"{end:hh\\:mm}");
        }

        private void SetValues()
        {
            ActualActivityTime = ToStringFormat(_actualActivityStart, _actualActivityEnd);
            NextActivityTime=ToStringFormat(_nextActivityStart, _nextActivityEnd);
            PreviousActivityTime = ToStringFormat(_previousActivityStart, _previousActivityEnd);
            Day=Enum.GetName(typeof(DayOfWeek),_actualShowedActivity.Day).ToString().ToUpper();
        }

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

        private Activity _actualActivity;
        private Activity _actualShowedActivity;

        private int _value;
        private List<Activity> _activities;
        private SqLiteService _sqLiteService;
        private PageService _pageService;

        public ActivityViewModel()
        {
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            ToRun();
        }

        public async void ToRun()
        {
            List<Activity> SQlitedata = _sqLiteService.ToListAsync().Result;
            if (SQlitedata.Count == 0)
            {
                var mrdka = new Dowloanding();
                mrdka.Download();
                Thread.Sleep(5000);
                _sqLiteService = new SqLiteService();
                Thread.Sleep(5000);
                SQlitedata = _sqLiteService.ToListAsync().Result;
            }
            if (SQlitedata.Count == 0)
            {
                await _pageService.DisplayAlert("Error", "problem with data", "OK");
            }
            else
            {
                _activities = new List<Activity>(SQlitedata);
            
                _actualActivity = _activities.Where(activity => activity.Day == (int) DateTime.Today.DayOfWeek)
                    .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
                Next = new Command(async () => await Add());
                Before = new Command(async () => await Previous());
                Actual = new Command(async () => NextAndPrevious(0));
                NextAndPrevious(0);
            }
        }

        public async Task Add()
        {
            _value++;
            NextAndPrevious(_value);
        }
        
        public async Task Previous()
        {
            _value--;
            NextAndPrevious(_value);
        }
        
        private async void NextAndPrevious(int nextItems)
        {
            int actualIndex = _activities.IndexOf(_actualActivity) + nextItems;
            if (actualIndex > _activities.Count-1)
                actualIndex = actualIndex-_activities.Count;
            if (actualIndex < 0)
                actualIndex = actualIndex + _activities.Count;
            _actualShowedActivity = _activities[actualIndex];
            
            int previousIndex = _activities.IndexOf(_actualShowedActivity)-1;
            if (previousIndex > _activities.Count)
                previousIndex = previousIndex-_activities.Count;
            if (previousIndex < 0)
                previousIndex = previousIndex+_activities.Count;
            Activity previousActivity = _activities[previousIndex];
            
            int nextIndex = _activities.IndexOf(_actualShowedActivity)+1;
            if (nextIndex > _activities.Count-1)
                nextIndex = nextIndex-_activities.Count;
            if (nextIndex < 0)
                nextIndex = nextIndex+_activities.Count;
            Activity nextActivity = _activities[nextIndex];
            
            _actualActivityStart = _actualShowedActivity.Start;
            _actualActivityEnd = _actualShowedActivity.End;
            ActualActivityName = _actualShowedActivity.Name;

            _previousActivityStart = previousActivity.Start;
            _previousActivityEnd = previousActivity.End;
            PreviousActivityName = previousActivity.Name;
            
            _nextActivityStart = nextActivity.Start;
            _nextActivityEnd = nextActivity.End;
            NextActivityName = nextActivity.Name;
            
            SetValues();
        }
    }
}