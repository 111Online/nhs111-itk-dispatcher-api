namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public interface IMessageService
    {
        bool MessageAlreadyExists(string messageId, string message);

        void StoreMessage(string id, string message);
    }
}