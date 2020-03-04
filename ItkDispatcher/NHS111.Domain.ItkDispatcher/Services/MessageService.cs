using NHS111.Domain.Itk.Dispatcher.Models;
using NHS111.Utils.Cryptography;

namespace NHS111.Domain.Itk.Dispatcher.Services {
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Itk.Dispatcher.Repositories;

    public class MessageService : IMessageService {
        private readonly IAzureStorageService _azureStorageService;
        private readonly IItkDispatchRequestRepository _requestRepository;

        public MessageService(IAzureStorageService azureStorageService,
            IItkDispatchRequestRepository requestRepository) {
            _azureStorageService = azureStorageService;
            _requestRepository = requestRepository;
        }

        public bool MessageAlreadyExists(string messageId, string message) {
            var journey = _azureStorageService.GetHash(messageId);
            if (journey == null) return false;

            var hashEngine = new ComputeHash();
            var messageHash = hashEngine.Compute(message);
            return hashEngine.Compare(journey.Hash, messageHash);
        }

        public void StoreMessage(string id, string message) {
            var hashEngine = new ComputeHash();
            var messageHash = hashEngine.Compute(message);
            _azureStorageService.AddHash(new Journey() {Id = id, Hash = messageHash});
        }

        public async Task StoreRequestAsync(string id, string message, CancellationToken cancellationToken) {
            await _requestRepository.InsertAsync(id, message, CancellationToken.None);
        }
    }

    public interface IMessageService {
        bool MessageAlreadyExists(string messageId, string message);
        void StoreMessage(string id, string message);
        Task StoreRequestAsync(string id, string message, CancellationToken cancellationToken);
    }
}
