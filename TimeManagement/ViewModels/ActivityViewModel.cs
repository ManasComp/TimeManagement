using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Helpers;
using TimeManagement.Interfaces;
using TimeManagement.Models;
using TimeManagement.Services;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ActivityViewModel : BaseViewModel, IHasCollectionViewModel
    {
        public IHasCollectionView View { get; set; }
        public ICommand Next { get; set; }
        public ICommand Before { get; set; }
        public ICommand Actual { get; set; }
        public ICommand Load { get; set; }
        private readonly CrashesHelper _crashesHelper;
        private readonly MessagingCenterHelper _messagingCenterHelper;
        
        private string _day;
        public string Day
        {
            set => SetValue(ref _day, value);
            get => _day;
        }
        
        private bool _isRefreshing;

        public bool IsRefreshing
        {
            set => SetValue(ref _isRefreshing, value);
            get => _isRefreshing;
        }
        
        private double _activitiesOpacity;
        public double ActivitiesOpacity
        {
            set => SetValue(ref _activitiesOpacity, value);
            get => _activitiesOpacity;
        }

        private ObservableCollection<ActivityVM> _collection;
        public ObservableCollection<ActivityVM> Collection
        {
            get => _collection;
            set
            {
                SetValue(ref _collection, value);
                if (_actualId >= 0 && _collection.Count>_actualId-1)
                {
                    _collection[_actualId].BackgroundSquareColor = Color.FromHex("#808080");
                    _collection[_actualId].BackgroundTextColor = Color.FromHex("#e1e1e1");
                }
            }
        }

        private int _actualId => Collection.IndexOf(_actualShowedActivity);
        private List<List<ActivityVM>> _programByDays = new List<List<ActivityVM>>();
        private ActivityVM _actualShowedActivity =>Collection
             .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
        private int _value;
        private int _dayOfWeek => (int) DateTime.Today.DayOfWeek;
        private SqLiteService _sqLiteService;
        private readonly PageService _pageService;
        private readonly Downloading _downloading;
        private List<Activity> _sQlitedata;

        public ActivityViewModel()
        {
            Collection = new ObservableCollection<ActivityVM>();
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            _downloading = new Downloading();
            _crashesHelper = new CrashesHelper();
            _messagingCenterHelper = new MessagingCenterHelper();
            _pageService.MessagingCenterSubscribe<ShellViewModel, ActivityViewModel>(this, _messagingCenterHelper.Refreshing, new Command(async () => await refresh()));
            _value = _dayOfWeek;
            
            uIsettings();
        }

        private async Task refresh()
        {
            IsRefreshing = !IsRefreshing;
            if (IsRefreshing)
            {
                ActivitiesOpacity = 0.2;
            }
            else
            {
                ActivitiesOpacity = 1;
            }
        }
        private async void dowlondData()
        {
            _sQlitedata = _sqLiteService.ToListAsync().Result;
            if (_sQlitedata.Count == 0)
            {
                await _downloading.Download();
                _sqLiteService = new SqLiteService();
                _sQlitedata = _sqLiteService.ToListAsync().Result;
            }
        }

        private void convertingToVm()
        {
            int day = 0;
            foreach (var activity in _sQlitedata)
            {
                if (_programByDays.Count == 0)
                {
                    _programByDays.Add(new List<ActivityVM>());
                    _programByDays[day].Add(new ActivityVM(_sQlitedata[_sQlitedata.Count - 1], TimeSpan.Zero, activity.End, day));
                    _programByDays[day].Add(new ActivityVM(activity));
                }
                else if ((activity.Name.Trim().ToLower() == "sleeping") &
                    (_sQlitedata.IndexOf(activity) != _sQlitedata.Count - 1))
                {
                    _programByDays.Add(new List<ActivityVM>());
                    day++;
                    _programByDays[day - 1].Add(new ActivityVM(activity, activity.Start, TimeSpan.FromHours(24), day - 1));
                    _programByDays[day].Add(new ActivityVM(activity, TimeSpan.Zero, activity.End, day));
                }
                else
                {
                    _programByDays[day].Add(new ActivityVM(activity));
                }
            }
            if (_programByDays.Count> _dayOfWeek)
                Collection = new ObservableCollection<ActivityVM>(_programByDays[_dayOfWeek]);
        }

        private void uIsettings()
        {
            IsRefreshing = true;
            ActivitiesOpacity = 1;
            Next = new Command(async () => await changeDay(true));
            Before = new Command(async () => await changeDay(false));
            Actual = new Command(async () => await goHome());
            Load = new Command(async  () => await ToLoad());
            Day = Enum.GetName(typeof(DayOfWeek), _dayOfWeek).ToString().ToUpper();
        }
        private async Task ToLoad()
        {
            try
            {
                dowlondData();
                convertingToVm();
                scrollToItem(_actualId);
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await _crashesHelper.TrackErrorAsync(ex);
            }
        }

        private async Task changeDay(bool Next)
        {
            if (Next == true)
            {
                if (_value > 5)
                    _value = 0;
                else
                    _value++;
            }

            else
            {
                if (_value < 1)
                    _value = 6;
                else
                    _value--;
            }
            _pageService.Vibrate();
            changeData();
        }

        private void changeData()
        {
            Day = Enum.GetName(typeof(DayOfWeek), _value).ToString().ToUpper();
            Collection = new ObservableCollection<ActivityVM>(_programByDays[_value]);
        }

        private async Task goHome()
        {
            if (_value != _dayOfWeek)
            {
                _value=_dayOfWeek;
                changeData();
            }
            scrollToItem(_actualId);
            await _pageService.Vibrate();
        }

        private void scrollToItem(int index)
        {
            if (index>=0)
                View.CollectionView.ScrollTo(_actualShowedActivity, null, ScrollToPosition.Center,true);
        }
    }
}