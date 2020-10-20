using FoodOrderApp.Services;
using FoodOrderApp.Services.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Helpers;
using TimeManagement.Models;
using TimeManagement.ViewModels;
using Xamarin.Forms;

namespace TimeManagement.Services
{
    public class ActivityViewModel : BaseViewModel
    {
        public ICommand Next { get; set; }
        public ICommand Before { get; set; }
        public ICommand Actual { get; set; }
        
        public ObservableCollection<ActivityVM> _activities { get; set; }
        
        private string _day;
        public string Day
        {
            set => SetValue(ref _day, value);
            get => _day;
        }

        private ObservableCollection<ActivityVM> _collection;
        public ObservableCollection<ActivityVM> Collection
        {
            get => _collection;
            set => SetValue(ref _collection, value);
        }
        ObservableCollection<ObservableCollection<ActivityVM>> kravina =
            new ObservableCollection<ObservableCollection<ActivityVM>>();
        
        private bool _homeIsEnabled;

        public bool HomeIsEnabled
        {
            set => SetValue(ref _homeIsEnabled, value);
            get => _homeIsEnabled;
        }

        public int actualId
        {
            get => Collection.IndexOf(Collection
                .Where(item => item.Day == _actualShowedActivity.Day)
                .Where(item => item.Duration == _actualShowedActivity.Duration)
                .Where(item => item.End == _actualShowedActivity.End)
                .Where(item => item.Id == _actualShowedActivity.Id)
                .Where(item => item.Name == _actualShowedActivity.Name)
                .Where(item => item.Start == _actualShowedActivity.Start)
                .Where(item => item.UserId == _actualShowedActivity.UserId)
                .FirstOrDefault());
        }
        public ActivityVM _actualShowedActivity { get; set; }

        private int _value = (int)DateTime.Today.DayOfWeek;

        private readonly SqLiteService _sqLiteService;
        private readonly PageService _pageService;
        private readonly Dowloanding _dowloanding;

        public ActivityViewModel()
        {
            Collection = new ObservableCollection<ActivityVM>();
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            _dowloanding = new Dowloanding();
            ToRun();
        }

        private string ToStringFormat(TimeSpan start, TimeSpan end)
        {
            return string.Format($"{start:hh\\:mm}" + " - " + $"{end:hh\\:mm}");
        }

        private async void SetValues()
        { 
            Day=Enum.GetName(typeof(DayOfWeek),_actualShowedActivity.Day).ToString().ToUpper();
        }

        public async Task ToRun()
        {
            try
            {
                List<Activity> sQlitedata = _sqLiteService.ToListAsync().Result;
                if (sQlitedata.Count == 0)
                {
                    await _dowloanding.Download();
                    sQlitedata = _sqLiteService.ToListAsync().Result;
                }
                else
                {
                    _activities = new ObservableCollection<ActivityVM>();
    
                    int day = 0;
                    foreach (var activity in sQlitedata)
                    {
                        if (activity.Name.Trim().ToLower() == "sleeping" & sQlitedata.IndexOf(activity)!=sQlitedata.Count-1)
                        {
                            kravina.Add(new ObservableCollection<ActivityVM>());
                            day++;
                            kravina[day - 1].Add(new ActivityVM(activity));
                        }

                        if (kravina.Count == 0)
                        {
                            kravina.Add(new ObservableCollection<ActivityVM>());
                            kravina[0].Add(new ActivityVM(sQlitedata[sQlitedata.Count-1]));
                        }

                        _activities.Add(new ActivityVM(activity));
                        kravina[day].Add(new ActivityVM(activity));
                    }

                    Collection = kravina[(int)DateTime.Today.DayOfWeek];
                    _actualShowedActivity = _activities.Where(activity => activity.Day == (int)DateTime.Today.DayOfWeek)
                        .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
                    Next = new Command(async () => await Add());
                    Before = new Command(async () => await Previous());
                    Actual = new Command(async () => await NextAndPrevious(0));
                    await NextAndPrevious(0);
                    Collection[actualId].BackgroundSquareColor = Color.FromHex("#808080");
                    Collection[actualId].BackgroundTextColor = Color.FromHex("#e1e1e1");
                }
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await CrashesHelper.TrackErrorAsync(ex);
            }

        }

        public async Task Add()
        {
            _value++;
            Collection=kravina[_value];
        }
        
        public async Task Previous()
        {
            _value--;
            Collection=kravina[_value];
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
            SetValues();
        }
    }
}