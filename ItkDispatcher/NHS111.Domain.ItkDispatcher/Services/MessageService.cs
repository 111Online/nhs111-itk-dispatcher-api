namespace NHS111.Domain.Itk.Dispatcher.Services
{
    using System;

    using Microsoft.WindowsAzure.Storage.Table;

    public class MessageService : IMessageService
    {
        private readonly IAzureStorageService _azureStorageService;

        private readonly string _currentPartition;

        private readonly string _previousPartition;

        public MessageService(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;

            _currentPartition = DateTime.Now.ToString("yyyy-MM");

            _previousPartition = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
        }

        public bool MessageAlreadyExists(string message)
        {
            var hashEngine = new HashService();

            var messageHash = hashEngine.Compute(message);
            
            var entityThisMonth= new TableEntity(_currentPartition, messageHash);
            
            var entityLastMonth = new TableEntity(_previousPartition, messageHash);
            
            return _azureStorageService.EntityExists(entityLastMonth) || _azureStorageService.EntityExists(entityThisMonth);
        }

        public void StoreMessage(string message)
        {
            try
            {
                var hashEngine = new HashService();

                var messageHash = hashEngine.Compute(message);

                var entity = new TableEntity
                                   {
                                       PartitionKey = _currentPartition, RowKey = messageHash
                                   };
                
                _azureStorageService.AddEntity(entity);
            }
            catch (Exception)
            {
                // TODO: Add logging to identify why the insert failed
            }
        }
    }
}
