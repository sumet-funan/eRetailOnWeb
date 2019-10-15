using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eProductOnWeb.Models
{
    public class RankingProfile
    {
        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }

        [DataMember(Name = "point")]
        public double Point { get; set; }

        [DataMember(Name = "hasBuffer")]
        public bool HasBuffer { get; set; }
    }
}
