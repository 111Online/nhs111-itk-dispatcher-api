using NHS111.Domain.Itk.Dispatcher.Services;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Domain.Itk.Dispatcher.IoC
{
    public class DispatcherDomainRegistry : Registry
    {
        public DispatcherDomainRegistry()
        {
            For<IAzureStorageService>().Use<AzureStorageService>().Singleton();

            For<IHashService>().Use<HashService>().Singleton();

            For<IMessageService>().Use<MessageService>().Singleton();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
