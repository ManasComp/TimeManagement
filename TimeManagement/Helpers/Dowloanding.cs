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
        private List<DayProgram> activities;
        private FirebaseService firebaseService;
        private SqLiteService service;

        public Dowloanding()
        {
            firebaseService = new FirebaseService();
            service = new SqLiteService();
        }

        public async void Dowloand()
        {
            activities = (await firebaseService.OnceAsync<List<DayProgram>>("DayPrograms")).FirstOrDefault();
            await service.DeleteAllAsync();
            foreach (DayProgram program in activities)
            {
                foreach (Activity activity in program)
                {
                    activity.Day = activities.IndexOf(program);
                    await service.InsertAsync(activity);
                }
            }
        }
    }
}