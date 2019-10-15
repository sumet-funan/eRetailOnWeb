using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using eProductOnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eProductOnWeb.Controllers
{
    public class UnderlordsController : Controller
    {
        private static HttpClient client = new HttpClient();

        public async Task<IActionResult> Index()
        {
            string path = @"https://eea963a2-cb0d-4f72-82a9-73650417333c.mock.pstmn.io/underlords/get";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<Ranking>>();

                Ranking ranking = new Ranking
                {
                    Day = "Total",
                    DayOfWeek = 99,
                    Data = GetTotalRankingProfile(result)
                };
                result.Add(ranking);
                return View(result);
            }

            List<Ranking> rankings = new List<Ranking> { new Ranking { Data = new List<RankingProfile>() } };
            return View(rankings);
        }

        private List<RankingProfile> GetTotalRankingProfile(List<Ranking> result)
        {
            return new List<RankingProfile>
                    {
                        new RankingProfile
                        {
                            UserName = "B",
                            Point = GetPointTatal(result, "B"),
                        },
                        new RankingProfile
                        {
                            UserName = "TOP",
                            Point = GetPointTatal(result, "TOP"),
                        },
                        new RankingProfile
                        {
                            UserName = "NEIL",
                            Point = GetPointTatal(result, "NEIL"),
                        },
                        new RankingProfile
                        {
                            UserName = "OAT",
                            Point = GetPointTatal(result, "OAT"),
                        },
                        new RankingProfile
                        {
                            UserName = "NOT",
                            Point = GetPointTatal(result, "NOT"),
                        }
                    };
        }

        private double GetPointTatal(List<Ranking> rankings, string userName)
        {
            double totalPoint = 0;
            foreach (Ranking ranking in rankings)
            {
                totalPoint += ranking.Data.Where(x => x.UserName == userName).Select(x => x.Point).FirstOrDefault();
            }
            return totalPoint;
        }
    }
}