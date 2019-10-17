using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using eProductOnWeb.Models;
using eProductOnWeb.Models.Underlords;
using Microsoft.AspNetCore.Mvc;

namespace eProductOnWeb.Controllers
{
    public class UnderlordsController : Controller
    {
        private static HttpClient _client = new HttpClient();

        public async Task<IActionResult> Index(int year, int month, int weekOfMonth)
        {
            string endpoint = @"https://eea963a2-cb0d-4f72-82a9-73650417333c.mock.pstmn.io/underlords/get";
            string queryString = $"?year={year}&month={month}&weekOfMonth={weekOfMonth}";
            string url = endpoint + queryString;
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Ranking>();

                WeekInformation ranking = new WeekInformation
                {
                    Day = "Total",
                    DayOfWeek = 99,
                    DayInformation = new List<DayInformation>
                    {
                        CreateDayInformation(result.WeekInformation, "B"),
                        CreateDayInformation(result.WeekInformation, "TOP"),
                        CreateDayInformation(result.WeekInformation, "NEIL"),
                        CreateDayInformation(result.WeekInformation, "OAT"),
                        CreateDayInformation(result.WeekInformation, "NOT")
                    }
                };
                result.WeekInformation.Add(ranking);



                return View(result);
            }

            Ranking rankings = new Ranking { WeekInformation = new List<WeekInformation> { new WeekInformation { DayInformation = new List<DayInformation>() } } };
            return View(rankings);
        }

        private DayInformation CreateDayInformation(List<WeekInformation> weekInformations, string userName)
        {
            return new DayInformation
            {
                UserName = userName,
                Point = GetPointTatal(weekInformations, userName),
            };
        }

        private double GetPointTatal(List<WeekInformation> weekInformations, string userName)
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