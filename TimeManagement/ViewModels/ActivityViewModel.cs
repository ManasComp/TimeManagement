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

        private bool _homeIsEnabled;

        public bool HomeIsEnabled
        {
            set => SetValue(ref _homeIsEnabled, value);
            get => _homeIsEnabled;
        }

        private Activity _actualActivity;
        
        private Activity _actualShowedActivity;
        private Activity _nextShowedActivity;
        private Activity _previousShowedActivity;

        private int _value;
        private List<Activity> _activities;
        private readonly SqLiteService _sqLiteService;
        private PageService _pageService;
        private Dowloanding _dowloanding;

        public ActivityViewModel()
        {
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            _dowloanding = new Dowloanding();
            ToRun();
        }
        
        public string ToStringFormat(TimeSpan start, TimeSpan end)
        {
            return string.Format($"{start:hh\\:mm}" + " - " + $"{end:hh\\:mm}");
        }

        private async void SetValues()
        {
            ActualActivityTime = ToStringFormat(_actualShowedActivity.Start, _actualShowedActivity.End);
            NextActivityTime=ToStringFormat(_nextShowedActivity.Start, _nextShowedActivity.End);
            PreviousActivityTime = ToStringFormat(_previousShowedActivity.Start, _previousShowedActivity.End);
            Day=Enum.GetName(typeof(DayOfWeek),_actualShowedActivity.Day).ToString().ToUpper();
        }

        public async Task ToRun()
        {
            List<Activity> SQlitedata = _sqLiteService.ToListAsync().Result;
            if (SQlitedata.Count == 0)
            {
                await _dowloanding.Download();
                SQlitedata = _sqLiteService.ToListAsync().Result;
            }
            else
            {
                _activities = new List<Activity>(SQlitedata);
            
                _actualActivity = _activities.Where(activity => activity.Day == (int) DateTime.Today.DayOfWeek)
                    .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
                Next = new Command(async () => await Add());
                Before = new Command(async () => await Previous());
                Actual = new Command(async () => await NextAndPrevious(0));
                await NextAndPrevious(0);
            }
        }

        public async Task Add()
        {
            _value++;
            await NextAndPrevious(_value);
        }
        
        public async Task Previous()
        {
            _value--;
            await NextAndPrevious(_value);
        }

        private int Activities(int actualIndex)
        {
            if (actualIndex > _activities.Count-1)
                actualIndex = actualIndex-_activities.Count;
            if (actualIndex < 0)
                actualIndex = actualIndex + _activities.Count;
            return actualIndex;
        }

        private async Task NextAndPrevious(int nextItems)
        {
            if (nextItems == 0)
            {
                HomeIsEnabled = false;
            }
            else
            {
                HomeIsEnabled = true;
            }
            int actualIndex = _activities.IndexOf(_actualActivity) + nextItems;
            _actualShowedActivity = _activities[Activities(actualIndex)];
            
            int previousIndex = _activities.IndexOf(_actualShowedActivity)-1;
            _previousShowedActivity = _activities[Activities(previousIndex)];
            
            int nextIndex = _activities.IndexOf(_actualShowedActivity)+1;
            _nextShowedActivity = _activities[Activities(nextIndex)];
            
            ActualActivityName = _actualShowedActivity.Name;
            PreviousActivityName = _previousShowedActivity.Name;
            NextActivityName = _nextShowedActivity.Name;
            
            SetValues();
        }
    }
}