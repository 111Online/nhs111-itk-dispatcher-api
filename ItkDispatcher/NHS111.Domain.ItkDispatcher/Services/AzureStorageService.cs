using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly CloudTable _table;

        public AzureStorageService()
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            _table = tableClient.GetTableReference(CloudConfigurationManager.GetSetting("StorageTableReference"));
            // Create the table if it doesn't exist.
            _table.CreateIfNotExists();
        }

        public Task<int> AddHash(Journey journey)
        {
            var insertOperation = TableOperation.Insert(journey);
            var tableResult = _table.ExecuteAsync(insertOperation);
            return Task.Run(() => { return tableResult.Id; });
        }

        public Task<Journey> GetHash(string journeyId)
        {
            var retrievedResult = _table.CreateQuery<Journey>().Where(j => j.Id == journeyId);
            return Task.Run(() => retrievedResult.FirstOrDefault());
        }
    }

    public interface IAzureStorageService
    {
        Task<int> AddHash(Journey journey);
        Task<Journey> GetHash(string journeyId);
    }
}
