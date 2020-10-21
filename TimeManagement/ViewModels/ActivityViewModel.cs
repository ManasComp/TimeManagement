using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeManagement.Helpers;
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

        private int _actualId => Collection.IndexOf(_actualShowedActivity);
        private List<List<ActivityVM>> _programByDays = new List<List<ActivityVM>>();
        private ActivityVM _actualShowedActivity;
        private int _value;
        private int _dayOfWeek => (int) DateTime.Today.DayOfWeek;
        private readonly SqLiteService _sqLiteService;
        private readonly PageService _pageService;
        private readonly Dowloanding _dowloanding;
        private List<Activity> _sQlitedata;

        public ActivityViewModel()
        {
            Collection = new ObservableCollection<ActivityVM>();
            _sqLiteService = new SqLiteService();
            _pageService = new PageService();
            _dowloanding = new Dowloanding();
            _value = _dayOfWeek;
            ToRun();
        }

        private async void dowlondData()
        {
            _sQlitedata = _sqLiteService.ToListAsync().Result;
            if (_sQlitedata.Count == 0)
            {
                await _dowloanding.Download();
                _sQlitedata = _sqLiteService.ToListAsync().Result;
            }
        }

        private void convertingToVm()
        {
            int day = 0;
            foreach (var activity in _sQlitedata)
            {
                if ((activity.Name.Trim().ToLower() == "sleeping") &
                    (_sQlitedata.IndexOf(activity) != _sQlitedata.Count - 1))
                {
                    _programByDays.Add(new List<ActivityVM>());
                    day++;
                    _programByDays[day - 1].Add(new ActivityVM(activity));
                }

                if (_programByDays.Count == 0)
                {
                    _programByDays.Add(new List<ActivityVM>());
                    _programByDays[0].Add(new ActivityVM(_sQlitedata[_sQlitedata.Count - 1]));
                }

                _programByDays[day].Add(new ActivityVM(activity));
            }
            Collection = new ObservableCollection<ActivityVM>(_programByDays[_dayOfWeek]);
        }

        private void uIsettings()
        {
            Next = new Command(async () => await changeDay(true));
            Before = new Command(async () => await changeDay(false));
            Actual = new Command(async () => await goHome());
            Collection[_actualId].BackgroundSquareColor = Color.FromHex("#808080");
            Collection[_actualId].BackgroundTextColor = Color.FromHex("#e1e1e1");
            Day = Enum.GetName(typeof(DayOfWeek), _dayOfWeek).ToString().ToUpper();
        }
        public async Task ToRun()
        {
            try
            {
                dowlondData();
                convertingToVm();
                _actualShowedActivity = Collection
                    .Where(activity => activity.Day == _dayOfWeek)
                    .LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
                uIsettings();
               await goHome();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                await CrashesHelper.TrackErrorAsync(ex);
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
        }
        
        private void scrollToItem(int index)
        {
            if (index>0)
                View.CollectionView.ScrollTo(index);
        }
    }
}