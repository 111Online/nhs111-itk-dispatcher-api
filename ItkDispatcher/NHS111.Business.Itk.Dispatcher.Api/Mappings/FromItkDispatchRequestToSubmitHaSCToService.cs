using AutoMapper;
using NHS111.Business.Itk.Dispatcher.Api.ItkDispatcherSOAPService;
using NHS111.Domain.Itk.Dispatcher.Models;
using Authentication = NHS111.Domain.Itk.Dispatcher.Models.Authentication;

namespace NHS111.Business.Itk.Dispatcher.Api.Mappings
{
    public class FromItkDispatchRequestToSubmitHaScToService : Profile
    {
        public override string ProfileName
        {
            get { return "FromITKDispatchRequestToSubmitHaSCToService"; }
        }

        protected override void Configure()
        {
            CreateMap<ItkDispatchRequest, SubmitHaSCToService>()
                .ForMember(dest => dest.SubmitEncounterToServiceRequest, opt => opt.MapFrom(src => src));
         
            CreateMap<Authentication, ItkDispatcherSOAPService.Authentication>();

        }
    }
}