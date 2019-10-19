using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProductOnWeb.Models.Underlords
{
    public class DayInformation
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Rating { get; set; }
        public double Point { get; set; }
        public bool HasBuffer { get; set; }

        public DayInformation()
        {

        }

        public DayInformation(string userName, double point)
        {
            UserName = userName;
            Point = point;
        }
    }
}
