using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eProductOnWeb.BusinessLogics.Storage.Container
{
    public class Blob
    {
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=underlords;AccountKey=+jElAWf6bgRsSZAIlefQ8+43phYUtEXPfsAN7it6nyHStGv4V1i2TeKez+2qjMvLSH/hs3WbYJe2i14F4q5FgQ==;EndpointSuffix=core.windows.net";
        private readonly IConfiguration _configuration;

        public Blob(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("ConnectionStorageBlob");
        }

        public string GetUnderlordsWeekRanking(string containerName, string fileName)
        {
            if (CloudStorageAccount.TryParse(_connectionString, out CloudStorageAccount storageAccount))
            {
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                string contents = cloudBlockBlob.DownloadTextAsync().Result;
                return contents;
            }
            else
            {
                return string.Empty;
            }
        }

        public void GetInformationFromStorageBlob()
        {
            ProcessAsync().GetAwaiter().GetResult();
        }

        private async Task ProcessAsync()
        {
            if (CloudStorageAccount.TryParse(_connectionString, out CloudStorageAccount storageAccount))
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
    }
}
