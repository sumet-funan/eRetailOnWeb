using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using eProductOnWeb.BusinessLogics.Storage.Container;
using eProductOnWeb.Models.Underlords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eProductOnWeb.Controllers
{
    public class UnderlordsController : Controller
    {
        private static HttpClient _client;
        private Blob _blob;
        private readonly IConfiguration _configuration;

        public UnderlordsController(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
        }

        public IActionResult Index(int year, int month, int weekOfMonth)
        {
            string fileName = $"underlords-{year}{month}-{weekOfMonth}.json";
            string containerName = "week-information";

            _blob = new Blob(_configuration);
            string contents = _blob.GetUnderlordsWeekRanking(containerName, fileName);
            Ranking result = JsonConvert.DeserializeObject<Ranking>(contents);

            WeekInformation ranking = new WeekInformation
            {
                Day = "Total",
                DayOfWeek = 99,
                DayInformation = new List<DayInformation>
                {
                    GetTotalPointOfWeek(result.WeekInformation, "B"),
                    GetTotalPointOfWeek(result.WeekInformation, "TOP"),
                    GetTotalPointOfWeek(result.WeekInformation, "NEIL"),
                    GetTotalPointOfWeek(result.WeekInformation, "OAT"),
                    GetTotalPointOfWeek(result.WeekInformation, "NOT")
                }
            };
            result.WeekInformation.Add(ranking);

            if (result == null)
            {
                result = new Ranking { WeekInformation = new List<WeekInformation> { new WeekInformation { DayInformation = new List<DayInformation>() } } };
            }
            return View(result);
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
                totalPoint += weekInformation.DayInformation.Where(x => x.UserName == userName).Select(x => x.Point).FirstOrDefault();
            }
            return totalPoint;
        }
    }
}