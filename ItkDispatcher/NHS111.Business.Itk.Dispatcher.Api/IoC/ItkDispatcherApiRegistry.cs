using System.ServiceModel;
using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using StructureMap;
using StructureMap.Graph;
using AutoMapperWebConfiguration = NHS111.Business.Itk.Dispatcher.Api.Mappings.AutoMapperWebConfiguration;

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    public class ItkDispatcherApiRegistry : Registry
    {
        public ItkDispatcherApiRegistry()
        {
            var configuration = new Itk.Dispatcher.Api.Configuration.Configuration();
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