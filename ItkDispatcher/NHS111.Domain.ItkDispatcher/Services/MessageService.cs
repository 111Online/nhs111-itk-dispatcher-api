using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
    using System;

    public class MessageService : IMessageService
    {
        private readonly IAzureStorageService _azureStorageService;

        public MessageService(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        public bool MessageAlreadyExists(string messageId, string message)
        {
            var journey = _azureStorageService.GetHash(messageId);

            if (journey == null)
            {
                return false;
            }

            var hashEngine = new HashService();

            var messageHash = hashEngine.Compute(message);

            return hashEngine.Compare(journey.Hash, messageHash);
        }

        public void StoreMessage(string id, string message)
        {
            try
            {
                var hashEngine = new HashService();

                var messageHash = hashEngine.Compute(message);

                _azureStorageService.AddHash(
                    new Journey
                     {
                         RowKey = id, 
                         Id = id,
                         Hash = messageHash
                     });
            }
            catch (Exception)
            {
                // TODO: Add logging to identify why the insert failed
            }
        }
    }
}
