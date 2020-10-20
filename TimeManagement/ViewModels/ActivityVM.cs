using System;

using TimeManagement.Models;
using Xamarin.Forms;

namespace TimeManagement.ViewModels
{
    public class ActivityVM
    {
        public ActivityVM(Activity activity)
        {
            Start = activity.Start;
            End = activity.End;
            Name = activity.Name;
            Day = activity.Day;
            Id = activity.Id;
            UserId = activity.UserId;
        }
        
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Name { get; set; }
        
        public int Day { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }

        public Color BackgroundTextColor { get; set; } = Color.FromHex("#676767");
        public Color BackgroundSquareColor { get; set; } = Color.FromHex("#333333");
        
        public string Duration
        {
            get => string.Format($"{Start:hh\\:mm}" + " - " + $"{End:hh\\:mm}");
        }
    }
}