using AutoMapper;

namespace NHS111.Business.Itk.Dispatcher.Api.Mappings
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new FromITKDispatchRequestToSubmitHaSCToService());
                cfg.AddProfile(new FromItkDispatchRequestToSubmitEncounterToServiceRequest());
            });
        }
    }
}