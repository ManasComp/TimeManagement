using System;

namespace TimeManagement.Models
{
    public class Activity
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}