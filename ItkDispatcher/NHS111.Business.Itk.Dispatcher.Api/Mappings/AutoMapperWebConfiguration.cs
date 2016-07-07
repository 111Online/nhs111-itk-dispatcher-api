using AutoMapper;

namespace NHS111.Business.Itk.Dispatcher.Api.Mappings
{
    public static class AutoMapperWebConfiguration
    {
        public static IMapper Mapper;
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FromItkDispatchRequestToSubmitHaScToService());
                cfg.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
            });
            Mapper = config.CreateMapper();
            config.AssertConfigurationIsValid();
        }
    }
}