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

        public Ranking GetWeekRanking(string containerName, string fileName)
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
                weekInfoTotalOfficial.DayInformation.Add(GetTotalPointOfPlayer(weekInfoMatchOfficial, dayInformation));
                weekInfoTotalInformal.DayInformation.Add(GetTotalPointOfPlayer(weekInfoMatchInformal, dayInformation));
            };

            ranking.WeekInformation.Add(weekInfoTotalOfficial);
            ranking.WeekInformation.Add(weekInfoTotalInformal);
        }

        public void GetPlayerRanking(Ranking result)
        {
            result.PlayerPoints = new List<DayPoint>();
            foreach (var playerInfo in result.WeekInformation.Select(x => x.DayInformation).FirstOrDefault())
            {
                foreach (var weekInfo in result.WeekInformation)
                {
                    var playerPoint = new DayPoint
                    {
                        PlayerInfo = new Player
                        {
                            Id = playerInfo.UserId,
                            Name = playerInfo.UserName
                        },
                        DayInfo = new Day
                        {
                            DayOfWeek = weekInfo.DayOfWeek,
                            MatchInformation = weekInfo.MatchInformation,
                            Name = weekInfo.Day
                        },
                        RoundBattle = new RoundBattle
                        {
                            MouthOfYear = result.Month,
                            WeekOfMonth = result.WeekOfMonth,
                            Year = result.Year
                        },
                        Point = weekInfo.DayInformation.Where(x => x.UserId == playerInfo.UserId).Select(x => x.Point).FirstOrDefault()
                    };
                    result.PlayerPoints.Add(playerPoint);
                }
            }
        }

        private DayInformation GetTotalPointOfPlayer(List<WeekInformation> weekInformations, DayInformation dayInfo)
        {
            return new DayInformation(dayInfo.UserId, dayInfo.UserName, CalculateTotalPoint(weekInformations, dayInfo.UserName));
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
