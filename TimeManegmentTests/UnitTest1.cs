using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using FoodOrderApp.Services.DatabaseService;
using NUnit.Framework;
using TimeManagement.Models;
using TimeManagement.Services;

namespace TimeManegmentTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            FirebaseService firebaseService = new FirebaseService();
            var activities = firebaseService.OnceAsync<List<DayProgram>>("DayPrograms").Result[0];
            Activity activita = activities[(int) DateTime.Today.DayOfWeek]
                .Where(activity => activity.Start <= DateTime.Now.TimeOfDay).LastOrDefault();
            Assert.AreEqual("sleeping", activita.Name);
            Assert.AreEqual(TimeSpan.FromMinutes(1335), activita.Start);

        }
    }
}