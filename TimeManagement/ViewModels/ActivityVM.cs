using System;
using TimeManagement.Models;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ActivityVM:BaseViewModel
    {
        public ActivityVM(Activity activity)
        {
            Start = activity.Start;
            End = activity.End;
            Name = activity.Name;
            Day = activity.Day;
            Id = activity.Id;
            UserId = activity.UserId;
            BackgroundTextColor = Color.FromHex("#676767");
            BackgroundSquareColor = Color.FromHex("#333333");
        }
        
        public ActivityVM(Activity activity, TimeSpan start, TimeSpan end, int day)
        {
            Start = start;
            End = end;
            Name = activity.Name;
            Day = activity.Day;
            Id = activity.Id;
            UserId = activity.UserId;
            BackgroundTextColor = Color.FromHex("#676767");
            BackgroundSquareColor = Color.FromHex("#333333");
        }
        
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }

        private Color _backgroundTextColor;
        public Color BackgroundTextColor
        {
            get => _backgroundTextColor;
            set => SetValue(ref _backgroundTextColor, value);
        }

        private Color _backgroundSquareColor;
        public Color BackgroundSquareColor
        {
            get => _backgroundSquareColor;
            set => SetValue(ref _backgroundSquareColor, value);
        }

        public string Duration
        {
            get => string.Format($"{Start:hh\\:mm}" + " - " + $"{End:hh\\:mm}");
        }
    }
}