using System.Collections.Generic;
using SQLite;

namespace TimeManagement.Models
{
    public class DayProgram : List<Activity>
    {

        public int Id { get; set; }
        public string Name{ get; set; }
    }
}