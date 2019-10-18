using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProductOnWeb.Models.Underlords
{
    public class Ranking
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int WeekOfMonth { get; set; }
        public List<WeekInformation> WeekInformation { get; set; }
    }
}
