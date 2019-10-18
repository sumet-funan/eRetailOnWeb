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
            GetWeekInformationTotalPoint(ranking);
            return ranking;
        }

        private void GetWeekInformationTotalPoint(Ranking ranking)
        {
            List<WeekInformation> weekInfoMatchOfficial = ranking.WeekInformation.Where(x => x.MatchInformation.Officially).ToList();
            List<WeekInformation> weekInfoMatchInformal = ranking.WeekInformation.Where(x => !x.MatchInformation.Officially).ToList();

            WeekInformation weekInfoTotalOfficial = new WeekInformation(99, "Total (Officially)", new List<DayInformation>());
            WeekInformation weekInfoTotalInformal = new WeekInformation(98, "Total (Informal)", new List<DayInformation>());

            WeekInformation weekInfo = ranking.WeekInformation.Where(x => x.DayOfWeek == 2).FirstOrDefault();
            foreach (var dayInformation in weekInfo.DayInformation)
            {
                weekInfoTotalOfficial.DayInformation.Add(GetTotalPointOfPlayer(weekInfoMatchOfficial, dayInformation.UserName));
                weekInfoTotalInformal.DayInformation.Add(GetTotalPointOfPlayer(weekInfoMatchInformal, dayInformation.UserName));
            };

            ranking.WeekInformation.Add(weekInfoTotalOfficial);
            ranking.WeekInformation.Add(weekInfoTotalInformal);
        }

        private DayInformation GetTotalPointOfPlayer(List<WeekInformation> weekInformations, string userName)
        {
            return new DayInformation(userName, CalculateTotalPoint(weekInformations, userName));
        }

        private double CalculateTotalPoint(List<WeekInformation> weekInformations, string userName)
        {
            double totalPoint = 0;
            foreach (WeekInformation weekInformation in weekInformations.Where(x => x.MatchInformation.Officially))
            {
                totalPoint += weekInformation.DayInformation.Where(x => x.UserName == userName).Select(x => x.Point).FirstOrDefault();
            }
            return totalPoint;
        }
    }
}
