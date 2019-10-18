using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eProductOnWeb.BusinessLogics.Storage.Container;
using eProductOnWeb.Models.Underlords;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eProductOnWeb.BusinessLogics.Underlords
{
    public class UnderlordsRanking
    {
        private IConfiguration _configuration;

        private Blob _blob;

        public UnderlordsRanking(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Ranking GetUnderlordsWeekRanking(string containerName, string fileName)
        {
            _blob = new Blob(_configuration);
            string contents = _blob.GetUnderlordsWeekRanking(containerName, fileName);
            Ranking ranking = JsonConvert.DeserializeObject<Ranking>(contents);
            GetWeekOnformationTotalPoint(ranking);
            return ranking;
        }

        private void GetWeekOnformationTotalPoint(Ranking ranking)
        {
            WeekInformation weekInfoTotal = new WeekInformation
            {
                Day = "Total",
                DayOfWeek = 99,
                DayInformation = new List<DayInformation>()
            };
            WeekInformation weekInfo = ranking.WeekInformation.Where(x => x.DayOfWeek == 2).FirstOrDefault();
            foreach (var dayInformation in weekInfo.DayInformation)
            {
                weekInfoTotal.DayInformation.Add(GetTotalPointOfWeek(ranking.WeekInformation, dayInformation.UserName));
            };

            ranking.WeekInformation.Add(weekInfoTotal);
        }

        private DayInformation GetTotalPointOfWeek(List<WeekInformation> weekInformations, string userName)
        {
            return new DayInformation
            {
                UserName = userName,
                Point = GetTotalPoint(weekInformations, userName),
            };
        }

        private double GetTotalPoint(List<WeekInformation> weekInformations, string userName)
        {
            double totalPoint = 0;
            foreach (WeekInformation weekInformation in weekInformations)
            {
                if (weekInformation.MatchInformation.Officially)
                {
                    totalPoint += weekInformation.DayInformation.Where(x => x.UserName == userName).Select(x => x.Point).FirstOrDefault();
                }
            }
            return totalPoint;
        }
    }
}
