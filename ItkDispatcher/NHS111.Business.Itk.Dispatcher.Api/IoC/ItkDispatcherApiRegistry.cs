using System.ServiceModel;

using log4net;
using log4net.Config;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.IoC;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    using NHS111.Business.Itk.Dispatcher.Api.Mappings;

    public class ItkDispatcherApiRegistry : Registry
    {
        public ItkDispatcherApiRegistry()
        {
            var configuration = new Configuration.Configuration();

            IncludeRegistry<DispatcherDomainRegistry>();

            For<MessageEngine>().Use(new MessageEngineClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(configuration.EsbEndpointUrl)));

            XmlConfigurator.Configure();

            For<ILog>().Use(LogManager.GetLogger("log"));

            AutoMapperWebConfiguration.Configure();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}