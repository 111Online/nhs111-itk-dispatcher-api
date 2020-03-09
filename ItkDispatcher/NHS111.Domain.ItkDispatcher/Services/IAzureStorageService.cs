namespace NHS111.Domain.Itk.Dispatcher.Services
{
    using NHS111.Domain.Itk.Dispatcher.Models;

    public interface IAzureStorageService
    {
        int AddHash(Journey journey);

        Journey GetHash(string journeyId);
    }
}