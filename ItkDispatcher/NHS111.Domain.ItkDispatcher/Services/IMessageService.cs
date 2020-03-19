namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public interface IMessageService
    {
        bool MessageAlreadyExists(string message);

        void StoreMessage(string message);
    }
}