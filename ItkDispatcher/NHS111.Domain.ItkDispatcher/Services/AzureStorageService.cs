using System;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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

        public void AddEntity(TableEntity entity)
        {
            if (!EntityExists(entity))
            {
                var insertOperation = TableOperation.InsertOrReplace(entity);

                _table.ExecuteAsync(insertOperation);
            }
        }

        public bool EntityExists(TableEntity entity)
        {
            try
            {
                var operation = TableOperation.Retrieve<TableEntity>(entity.PartitionKey, entity.RowKey);
                
                var result = _table.Execute(operation);

                return result.Result != null;
            }
            catch (Exception)
            {
                // TODO: this should be logged and an appropriate response returned to the client
                return false;
            }
        }
    }
}
