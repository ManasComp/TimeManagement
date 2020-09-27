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
    public class ActivityViewModel:BaseViewModel
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

        private Activity _actualActivity;


        private int _value;
        private List<Activity> _activities;
        private readonly SqLiteService _sqLiteService;
        private PageService _pageService;

        public ActivityViewModel()
        {
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            var mrdka = _sqLiteService.ToListAsync().Result;
            if (mrdka.Count == 0)
            {
                new SettingsViewModel().Refresh.Execute(null);
                mrdka = _sqLiteService.ToListAsync().Result;
            }
            _activities = new List<Activity>(mrdka);
            
            _actualActivity = _activities.Where(activity => activity.Day == (int) DateTime.Today.DayOfWeek)
                .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
            Next = new Command(async () => await Add());
            Before = new Command(async () => await Previous());
            Actual = new Command(async () => await Default());
            Default();
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
        
        private async Task Default()
        {
            _value = 0;
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
            
            Day=DateTime.Today.DayOfWeek.ToString().ToUpper();
        }

        private Activity actualShowedActivity;
        private async void NextAndPrevious(int nextItems)
        {
            int actualIndex = _activities.IndexOf(_actualActivity) + nextItems;
            if (actualIndex > _activities.Count-1)
                actualIndex = actualIndex-_activities.Count;
            if (actualIndex < 0)
                actualIndex = actualIndex + _activities.Count;
            actualShowedActivity = _activities[actualIndex];
            
            int previousIndex = _activities.IndexOf(actualShowedActivity)-1;
            if (previousIndex > _activities.Count)
                previousIndex = previousIndex-_activities.Count;
            if (previousIndex < 0)
                previousIndex = previousIndex+_activities.Count;
            Activity previousActivity = _activities[previousIndex];
            
            int nextIndex = _activities.IndexOf(actualShowedActivity)+1;
            if (nextIndex > _activities.Count-1)
                nextIndex = nextIndex-_activities.Count;
            if (nextIndex < 0)
                nextIndex = nextIndex+_activities.Count;
            Activity nextActivity = _activities[nextIndex];
            
            _actualActivityStart = actualShowedActivity.Start;
            _actualActivityEnd = actualShowedActivity.End;
            ActualActivityName = actualShowedActivity.Name;

            _previousActivityStart = previousActivity.Start;
            _previousActivityEnd = previousActivity.End;
            PreviousActivityName = previousActivity.Name;
            
            _nextActivityStart = nextActivity.Start;
            _nextActivityEnd = nextActivity.End;
            NextActivityName = nextActivity.Name;
            
            Day=Enum.GetName(typeof(DayOfWeek),actualShowedActivity.Day).ToString().ToUpper();
            //Day = String.Format("{0}, {1}, {2}", previousIndex, actualIndex, nextIndex);
        }
    }
}