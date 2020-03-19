namespace NHS111.Domain.Itk.Dispatcher.Services
{
    using Microsoft.WindowsAzure.Storage.Table;

    public interface IAzureStorageService
    {
        void AddEntity(TableEntity entity);

        bool EntityExists(TableEntity entity);
    }
}