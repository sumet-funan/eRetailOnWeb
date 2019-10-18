using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using eProductOnWeb.Models;
using eProductOnWeb.Models.Underlords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

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

                GetInformationFromStorageBlob();

                return View(result);
            }

            Ranking rankings = new Ranking { WeekInformation = new List<WeekInformation> { new WeekInformation { DayInformation = new List<DayInformation>() } } };
            return View(rankings);
        }

        private void GetInformationFromStorageBlob()
        {
            ProcessAsync().GetAwaiter().GetResult();
        }

        private static async Task ProcessAsync()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=underlords;AccountKey=+jElAWf6bgRsSZAIlefQ8+43phYUtEXPfsAN7it6nyHStGv4V1i2TeKez+2qjMvLSH/hs3WbYJe2i14F4q5FgQ==;EndpointSuffix=core.windows.net";

            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("week-rule");
                await cloudBlobContainer.CreateIfNotExistsAsync();

                var permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);

                var file = new FileStream(@"C:\Engr_CodeRepo\SourceCode\eRetailOnWeb\src\eRetailOnWeb\Files\rule-201910-3.json", FileMode.Open);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference("rule-201910-3.json");
                cloudBlockBlob.Properties.ContentType = "application/json";
                await cloudBlockBlob.UploadFromStreamAsync(file);
                file.Close();
            }
            else
            {
                Console.WriteLine("The connection string isn't valid");
                Console.WriteLine("Press any key to exit the application.");
            }
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