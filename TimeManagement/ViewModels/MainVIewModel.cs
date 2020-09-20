using System;
using System.Collections.Generic;
using System.Linq;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Helpers;
using TimeManagement.Models;

namespace TimeManagement.Services
{
    public class MainVIewModel
    {
        public string Time => string.Format(Start.Hours + ":" + Start.Minutes + "-" + End.Hours + ":" + End.Minutes);
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Name { get; set; }
        private SqLiteService _sqLiteService;

        public MainVIewModel()
        {
            _sqLiteService = new SqLiteService();
            set();
        }

        public async void set()
        {
            Dowloanding dowloanding = new Dowloanding();
            dowloanding.Dowloand();
            List<Activity> blbost = new List<Activity>(await _sqLiteService.ToListAsync());
             Activity activita = blbost.Where(activity=> activity.Id==(int) DateTime.Today.DayOfWeek)
                 .Where(activity => activity.Start <= DateTime.Now.TimeOfDay).LastOrDefault();
            Start = activita.Start;
            End = activita.End;
            Name = activita.Name;
        }

    }
}