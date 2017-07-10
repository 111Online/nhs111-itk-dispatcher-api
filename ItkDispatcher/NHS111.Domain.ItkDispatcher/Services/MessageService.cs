using System.Threading.Tasks;
using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Utils.Cryptography;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
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
            if (journey == null) return false;

            var hashEngine = new ComputeHash();
            var messageHash = hashEngine.Compute(message);
            return hashEngine.Compare(journey.Hash, messageHash);
        }

        public void StoreMessage(string id, string message)
        {
            var hashEngine = new ComputeHash();
            var messageHash = hashEngine.Compute(message);
            _azureStorageService.AddHash(new Journey() {Id = id, Hash = messageHash});
        }
    }

    public interface IMessageService
    {
        bool MessageAlreadyExists(string messageId, string message);
        void StoreMessage(string id, string message);
    }
}
