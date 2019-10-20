using System;
using System.Collections.Generic;
using System.Text;

namespace eProductOnWeb.Models.Underlords
{
    public class DayPoint
    {
        public int Id { get; set; }
        public RoundBattle RoundBattle { get; set; }
        public double Point { get; set; }
        public Day DayInfo { get; set; }
        public Player PlayerInfo { get; set; }
    }
}
