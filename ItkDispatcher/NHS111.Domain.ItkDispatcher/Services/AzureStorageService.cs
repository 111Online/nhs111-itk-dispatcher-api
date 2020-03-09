using System;
using System.Linq;

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
            
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            
            var tableClient = storageAccount.CreateCloudTableClient();
            
            _table = tableClient.GetTableReference(CloudConfigurationManager.GetSetting("StorageTableReference"));
            
            _table.CreateIfNotExists();
        }

        public int AddHash(Journey journey)
        {
            var existingJourney = GetHash(journey.RowKey);

            if (existingJourney != null)
            {
                existingJourney.Hash = journey.Hash;

            }
            else
            {
                existingJourney = journey;
            }

            var insertOperation = TableOperation.InsertOrReplace(existingJourney);

            var tableResult = _table.ExecuteAsync(insertOperation);

            return tableResult.Id;
        }

        public Journey GetHash(string rowKey)
        {
            try
            {
                var partitionKey = DateTime.Now.ToString("yyyy-MM");
                
                var operation = TableOperation.Retrieve<Journey>(partitionKey, rowKey);

                var result = _table.Execute(operation);

                var journey = result.Result as Journey;

                return journey;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
