using System;
using System.Collections.Generic;
using System.Text;

namespace eProductOnWeb.Models.Underlords
{
    public class Day
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DayOfWeek { get; set; }
        public MatchInformation MatchInformation { get; set; }
    }
}
