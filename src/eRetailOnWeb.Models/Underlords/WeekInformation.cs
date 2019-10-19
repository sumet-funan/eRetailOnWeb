using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProductOnWeb.Models.Underlords
{
    public class WeekInformation
    {
        public int DayOfWeek { get; set; }
        public string Day { get; set; }
        public MatchInformation MatchInformation { get; set; }
        public List<DayInformation> DayInformation { get; set; }

        public WeekInformation()
        {

        }

        public WeekInformation(int dayOfWeek, string day, List<DayInformation> dayInformation)
        {
            DayOfWeek = dayOfWeek;
            Day = day;
            DayInformation = dayInformation;
        }
    }
}
