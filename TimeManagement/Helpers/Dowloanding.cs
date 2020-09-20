using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FoodOrderApp.Services.DatabaseService;
using TimeManagement.Models;
using TimeManagement.Services;

namespace TimeManagement.Helpers
{
    public class Dowloanding
    {
        List<DayProgram> activities;
        private List<Activity> mrdka =new List<Activity>();

        public async void Dowloand()
        {
            FirebaseService firebaseService = new FirebaseService();
            List<Activity> mrdka = new List<Activity>();
            activities = (await firebaseService.OnceAsync<List<DayProgram>>("DayPrograms")).FirstOrDefault();
            SqLiteService service = new SqLiteService();
            await service.DeleteAllAsync();
            foreach (DayProgram program in activities)
            {
                foreach (Activity activity in program)
                {
                    activity.Day = activities.IndexOf(program);
                    mrdka.Add(activity);
                }
            }

            foreach (Activity activity in mrdka)
            {
                await service.InsertAsync(activity);
            }
        }
    }
}