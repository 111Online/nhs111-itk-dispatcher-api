using System.ServiceModel;
using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using StructureMap;
using StructureMap.Graph;
using AutoMapperWebConfiguration = NHS111.Business.Itk.Dispatcher.Api.Mappings.AutoMapperWebConfiguration;
using NHS111.Domain.Itk.Dispatcher.IoC;

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    public class ItkDispatcherApiRegistry : Registry
    {
        public ItkDispatcherApiRegistry()
        {
            var configuration = new Configuration.Configuration();
            IncludeRegistry<DispatcherDomainRegistry>();
            For<MessageEngine>().Use(new MessageEngineClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(configuration.EsbEndpointUrl)));
            AutoMapperWebConfiguration.Configure();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}