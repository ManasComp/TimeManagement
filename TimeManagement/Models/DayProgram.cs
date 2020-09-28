using System.Collections.Generic;

namespace TimeManagement.Models
{
    public class DayProgram : List<Activity>
    {

        public int Id { get; set; }
        public string Name{ get; set; }
    }
}