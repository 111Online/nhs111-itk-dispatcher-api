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

namespace NHS111.Business.Itk.Dispatcher.Api.IoC {
    using System.Configuration;
    using System.Data.SQLite;
    using Data.Itk.Dispatcher.Repositories;
    using Data.Sqlite.Itk.Dispatcher.Repositories;
    using Domain.Itk.Dispatcher.Models;
    using Configuration = Configuration.Configuration;

    public class ItkDispatcherApiRegistry : Registry {
        public ItkDispatcherApiRegistry() {
            var logger = LogManager.GetLogger("log");
            var configuration = new Configuration();


            IncludeRegistry<DispatcherDomainRegistry>();
            var itkDispatchRequestRepository = new ItkDispatchRequestRepository(configuration.ConnectionString, logger);
            itkDispatchRequestRepository.InitialiseDatabase();
            For<IItkDispatchRequestRepository>()
                .Use(itkDispatchRequestRepository);
            For<MessageEngine>().Use(new MessageEngineClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport),
                new EndpointAddress(configuration.EsbEndpointUrl)));
            XmlConfigurator.Configure();
            For<ILog>().Use(logger);
            AutoMapperWebConfiguration.Configure();
            Scan(scan => {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }

    }
}