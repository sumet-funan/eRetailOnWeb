using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eProductOnWeb.Models
{
    public class Ranking
    {
        [DataMember(Name = "dayOfWeek")]
        public int DayOfWeek { get; set; }

        [DataMember(Name = "day")]
        public string Day { get; set; }

        [DataMember(Name = "data")]
        public List<RankingProfile> Data { get; set; }
    }
}
