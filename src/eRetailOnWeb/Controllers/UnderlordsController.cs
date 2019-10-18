using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using eProductOnWeb.BusinessLogics.Storage.Container;
using eProductOnWeb.BusinessLogics.Underlords;
using eProductOnWeb.Models.Underlords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eProductOnWeb.Controllers
{
    public class UnderlordsController : Controller
    {
        private static HttpClient _client;
        private UnderlordsRanking _underlordsRanking;
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

            _underlordsRanking = new UnderlordsRanking(_configuration);
            Ranking result = _underlordsRanking.GetUnderlordsWeekRanking(containerName, fileName);

            if (result == null)
            {
                result = new Ranking { WeekInformation = new List<WeekInformation> { new WeekInformation { DayInformation = new List<DayInformation>() } } };
            }
            return View(result);
        }
    }
}