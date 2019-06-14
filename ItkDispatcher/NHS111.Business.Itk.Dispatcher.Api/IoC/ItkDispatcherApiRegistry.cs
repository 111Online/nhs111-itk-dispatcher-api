using System.ServiceModel;
using AutoMapper;
using log4net;
using log4net.Config;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using StructureMap;
using StructureMap.Graph;
using AutoMapperWebConfiguration = NHS111.Business.Itk.Dispatcher.Api.Mappings.AutoMapperWebConfiguration;
using NHS111.Domain.Itk.Dispatcher.IoC;
using NHS111.Utils.Logging;

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
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