using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;

namespace TimeManagement.Services
{
    public class MainVIewModel
    {
        public string actualActivityTime => string.Format(actualActivityStart.Hours + ":" + actualActivityStart.Minutes + "-" + actualActivityEnd.Hours + ":" + actualActivityEnd.Minutes);
        public string nextActivityTime => string.Format(nextActivityStart.Hours + ":" + nextActivityStart.Minutes + "-" + nextActivityEnd.Hours + ":" + nextActivityEnd.Minutes);
        public string previousActivityTime => string.Format(previousActivityStart.Hours + ":" + previousActivityStart.Minutes + "-" + previousActivityEnd.Hours + ":" + previousActivityEnd.Minutes);
        public TimeSpan actualActivityStart { get; set; }
        public TimeSpan actualActivityEnd { get; set; }
        public string ActualActivityName { get; set; }
        public string Day => DateTime.Today.DayOfWeek.ToString().ToUpper();

        public TimeSpan previousActivityStart { get; set; }
        public TimeSpan previousActivityEnd { get; set; }
        public string previousActivityName { get; set; }
        
        public TimeSpan nextActivityStart { get; set; }

        public string nextActivityName { get; set; }

        public TimeSpan nextActivityEnd { get; set; }
        private SqLiteService _sqLiteService;

        public MainVIewModel()
        {
            _sqLiteService = new SqLiteService();
            set();
        }

        public async Task refresh()
        {
            Dowloanding dowloanding = new Dowloanding();
            dowloanding.Dowloand();
        }

        public async void set()
        {
            List<Activity> activities = new List<Activity>(await _sqLiteService.ToListAsync());
            Activity actualActivity = activities
                .Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).LastOrDefault(activity => activity.Start <= DateTime.Now.TimeOfDay);
            Activity previousActivity = activities[activities.IndexOf(actualActivity)-1];
                // .Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).Where(activity => activity.Start <= DateTime.Now.TimeOfDay).Reverse().Skip(1).Take(1).FirstOrDefault();
            Activity nextActivity = activities[activities.IndexOf(actualActivity)+1];
                //*Where(activity => activity.Id==(int) DateTime.Today.DayOfWeek).FirstOrDefault(activity => activity.Start >= DateTime.Now.TimeOfDay);
            actualActivityStart = actualActivity.Start;
            actualActivityEnd = actualActivity.End;
            ActualActivityName = actualActivity.Name;

            previousActivityStart = previousActivity.Start;
            previousActivityEnd = previousActivity.End;
            previousActivityName = previousActivity.Name;
            
            nextActivityStart = nextActivity.Start;
            nextActivityEnd = nextActivity.End;
            nextActivityName = nextActivity.Name;

        }
    }
}